using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object pooling utility for instantiateable monobehaviours.
/// Assumes that being inactive means it is ready to be reused again, after resetting.
/// </summary>
/// <typeparam name="T">The Type of ObjectPool this is.</typeparam>
public class ObjectPool<T> where T : MonoBehaviour
{
    /// <summary>
    /// List of all of the objects managed by this ObjectPool.
    /// Includes inactive and active objects.
    /// </summary>
    List<T> pooledObjects { get; set; } = new List<T>();

    /// <summary>
    /// An instance of the prefab to spawn.
    /// </summary>
    T prefabModel { get; set; }

    /// <summary>
    /// Transform parent for objects in this pool.
    /// </summary>
    Transform parent { get; set; }

    public ObjectPool(T prefab, int prewarmAmount = 0)
    {
        prefabModel = prefab;
        parent = new GameObject($"{prefab.name} pool").transform;
        Prewarm(prewarmAmount);
    }

    /// <summary>
    /// Gets an object from the pool.
    /// If there are any inactive objects, gets those.
    /// If there aren't, creates a new instance of the object based on the prefab.
    /// </summary>
    /// <returns>A pooled object. Returns it in the active state.</returns>
    public T GetObject()
    {
        for (int ii = 0; ii < pooledObjects.Count; ii++)
        {
            T pooledObject = pooledObjects[ii];

            // If the object doesn't exist anymore (it was destroyed, or we changed scenes), remove it from the list
            if (pooledObject == null)
            {
                pooledObjects.RemoveAt(ii--);
                continue;
            }

            if (!pooledObject.gameObject.activeSelf)
            {
                pooledObject.gameObject.SetActive(true);
                return pooledObject;
            }
        }

        // if we've reached here, there weren't any ready objects; make a new one
        T newObject = InstantiateObject(true);
        return newObject;
    }

    /// <summary>
    /// Creates instances of the prefab until there are <paramref name="amount"/> instances.
    /// They'll all be inactive.
    /// </summary>
    /// <param name="amount">The amount of instances to make.</param>
    public void Prewarm(int amount)
    {
        for (int ii = 0; ii < amount && pooledObjects.Count < amount; ii++)
        {
            InstantiateObject(false);
        }
    }

    /// <summary>
    /// Creates a new instance of a prefab and puts it in to the pool.
    /// </summary>
    /// <param name="active">Should the object be active when created?</param>
    /// <returns>The instance created.</returns>
    private T InstantiateObject(bool active)
    {
        T instance = GameObject.Instantiate(prefabModel, parent);
        pooledObjects.Add(instance);
        instance.gameObject.SetActive(active);
        return instance;
    }
}