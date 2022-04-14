using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/EventComponents/TownGotoEvent")]
public class TownGotoEvent : ObstacleEventComponent
{
    public override bool CloseCurrentState => true;
    public override IGameplayState GetNewState(SaveData activeSaveData, Action<int> setPointer)
    {
        activeSaveData.Day++;
        return new TownState();
    }
}
