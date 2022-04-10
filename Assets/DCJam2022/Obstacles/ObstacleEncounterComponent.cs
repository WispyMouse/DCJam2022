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
        setPointer(EventId + 1);
        return new BattleState(Foes);
    }
}
