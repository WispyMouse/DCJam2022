using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A handler class for accessing a global set of object pools.
/// Uses the prefab provided as the key to each pool; if you share the same prefab, it'll use the same pool.
/// </summary>
public static class GlobalObjectPool
{
    /// <summary>
    /// A dictionary map of prefab instance ids to object pools.
    /// The value is an object, but is always an ObjectPool<T>; we just don't know what type of T until we're fetching the object.
    /// </summary>
    static Dictionary<int, object> ObjectPools { get; set; } = new Dictionary<int, object>();

    /// <summary>
    /// Fetches an ObjectPool of type T from the static global pool.
    /// </summary>
    /// <typeparam name="T">The type of object pool to get.</typeparam>
    /// <param name="prefab">The prefab used as a key. This prefab will be used to instantiate new copies.</param>
    /// <param name="prewarmAmount">If there isn't a pool of this type, a new ObjectPool<T> will be created. It'll prewarm this many instances of the prefab.</param>
    /// <returns>An ObjectPool of the matching type.</returns>
    public static ObjectPool<T> GetObjectPool<T>(T prefab, int prewarmAmount = 0) where T : MonoBehaviour
    {
        object pool;
        if (ObjectPools.TryGetValue(prefab.GetInstanceID(), out pool))
        {
            return pool as ObjectPool<T>;
        }

        // Before instantiating any objects, put an empty pool in to the pool dictionary
        // this way if the instantiated object asks for a pool, it'll return the new empty one rather than create a new pool
        ObjectPool<T> newPool = new ObjectPool<T>(prefab);
        ObjectPools.Add(prefab.GetInstanceID(), newPool);
        newPool.Prewarm(prewarmAmount);
        return newPool;
    }
}