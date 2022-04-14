using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/EventComponents/ObstacleDialogueComponent")]
public class ObstacleDialogueComponent : ObstacleEventComponent
{
    public string Text;

    public override IGameplayState GetNewState(SaveData activeSaveData)
    {
        return new MessageBoxState(Text);
    }

    public override int AfterStateSetPointer(SaveData activeSaveData)
    {
        return EventId + 1;
    }
}