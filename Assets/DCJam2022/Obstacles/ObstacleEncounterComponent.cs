using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/ObstacleEncounterComponent")]
public class ObstacleEncounterComponent : ObstacleEventComponent
{
    public EncounterBattle Foes;

    public override IGameplayState GetNewState(Action<int> setPointer)
    {
        return new BattleState(Foes);
    }

    public override void AfterStateSetPointer(Action<int> setPointer)
    {
        if (BattleState.LastWasVictory)
        {
            setPointer(EventId + 1);
        }
        else
        {
            setPointer(-1);
        }
    }
}
