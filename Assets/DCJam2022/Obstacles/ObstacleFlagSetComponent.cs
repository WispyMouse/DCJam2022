using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/EventComponents/FlagSet")]
public class ObstacleFlagSet : ObstacleEventComponent
{
    public string FlagToSet;
    public int Value;

    public override IGameplayState GetNewState(SaveData activeSaveData, Action<int> setPointer)
    {
        activeSaveData.SetFlag(FlagToSet, Value);
        return null;
    }
}
