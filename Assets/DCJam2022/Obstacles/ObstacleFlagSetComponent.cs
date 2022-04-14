using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/EventComponents/FlagSet")]
public class ObstacleFlagSetComponent : ObstacleEventComponent
{
    public string FlagToSet;
    public int Value;

    public override IGameplayState GetNewState(SaveData activeSaveData)
    {
        activeSaveData.SetFlag(FlagToSet, Value);
        return null;
    }

    public override int AfterStateSetPointer(SaveData activeSaveData)
    {
        return EventId + 1;
    }
}
