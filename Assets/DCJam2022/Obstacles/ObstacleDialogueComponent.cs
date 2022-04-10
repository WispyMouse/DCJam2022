using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/ObstacleDialogueComponent")]
public class ObstacleDialogueComponent : ObstacleEventComponent
{
    public string Text;

    public override IGameplayState GetNewState(Action<int> setPointer)
    {
        setPointer(EventId + 1);
        return new MessageBoxState(Text);
    }
}