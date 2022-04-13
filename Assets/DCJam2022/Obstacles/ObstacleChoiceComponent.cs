using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/EventComponents/ObstacleChoiceComponent")]
public class ObstacleChoiceComponent : ObstacleEventComponent
{
    public string Text;
    public List<ObstacleChoiceEntry> Entries;

    public override IGameplayState GetNewState(SaveData activeSaveData, Action<int> setPointer)
    {
        return new ChoiceState(setPointer, this);
    }
}
