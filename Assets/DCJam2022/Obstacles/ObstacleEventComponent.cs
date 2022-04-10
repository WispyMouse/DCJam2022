using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleComponentType { Choice, Dialogue, Encounter, None }

public abstract class ObstacleEventComponent : ScriptableObject
{
    public int EventId;

    public abstract IGameplayState GetNewState(Action<int> setPointer);
}
