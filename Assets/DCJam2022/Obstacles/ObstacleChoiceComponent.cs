using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/EventComponents/ObstacleChoiceComponent")]
public class ObstacleChoiceComponent : ObstacleEventComponent
{
    public string Text;
    public List<ObstacleChoiceEntry> Entries;

    int chosenResult { get; set; }

    public override IGameplayState GetNewState(SaveData activeSaveData)
    {
        return new ChoiceState((int value) => { chosenResult = value; }, this);
    }

    public override int AfterStateSetPointer(SaveData activeSaveData)
    {
        return chosenResult;
    }
}
