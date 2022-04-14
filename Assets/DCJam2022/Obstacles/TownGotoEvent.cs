using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/EventComponents/TownGotoEvent")]
public class TownGotoEvent : ObstacleEventComponent
{
    public override bool CloseCurrentState => true;
    public override IGameplayState GetNewState(SaveData activeSaveData)
    {
        activeSaveData.Day++;
        return new TownState();
    }

    public override int AfterStateSetPointer(SaveData activeSaveData)
    {
        return -1;
    }
}
