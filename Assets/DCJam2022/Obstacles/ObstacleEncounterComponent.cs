using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/EventComponents/ObstacleEncounterComponent")]
public class ObstacleEncounterComponent : ObstacleEventComponent
{
    public EncounterBattle Foes;

    public override IGameplayState GetNewState(SaveData activeSaveData)
    {
        return new BattleState(Foes);
    }

    public override int AfterStateSetPointer(SaveData activeSaveData)
    {
        if (BattleState.LastWasVictory)
        {
            return EventId + 1;
        }
        else
        {
            return -1;
        }
    }
}
