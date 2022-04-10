using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoeMember : CombatMember
{
    public Foe Visual { get; set; }
    public FoeBattleData BattleData { get; set; }
    public int MaxProblemJuice { get; set; }
    public int CurProblemJuice { get; set; }

    public int CurPhase { get; set; } = 0;

    public FoeMember()
    {

    }

    public FoeMember(FoeEncounterPhase foe)
    {
        BattleData = foe.EncounteredFoe;
        MaxProblemJuice = foe.EncounteredFoe.Health;
        CurPhase = foe.FoeStartingPhase;
    }

    public void GoNextPhase()
    {
        if (BattleData == null)
        {
            return;
        }

        CurPhase = (CurPhase + 1) % BattleData.AttackPhases.Count;
    }
}