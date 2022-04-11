using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/TownGotoEvent")]
public class TownGotoEvent : ObstacleEventComponent
{
    public override bool CloseCurrentState => true;
    public override IGameplayState GetNewState(Action<int> setPointer)
    {
        return new TownState();
    }
}
