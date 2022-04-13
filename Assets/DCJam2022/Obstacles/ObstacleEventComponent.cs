using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleComponentType { Choice, Dialogue, Encounter, None }

public abstract class ObstacleEventComponent : ScriptableObject
{
    public int EventId;

    public virtual bool CloseCurrentState => false;

    public abstract IGameplayState GetNewState(SaveData activeSaveData, Action<int> setPointer);
    public virtual void AfterStateSetPointer(SaveData activeSaveData, Action<int> setPointer)
    {

    }
}
