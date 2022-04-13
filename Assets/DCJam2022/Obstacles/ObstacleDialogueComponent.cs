using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/EventComponents/ObstacleDialogueComponent")]
public class ObstacleDialogueComponent : ObstacleEventComponent
{
    public string Text;

    public override IGameplayState GetNewState(SaveData activeSaveData, Action<int> setPointer)
    {
        setPointer(EventId + 1);
        return new MessageBoxState(Text);
    }
}