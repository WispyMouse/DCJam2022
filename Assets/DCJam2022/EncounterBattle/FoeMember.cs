using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoeMember : CombatMember
{
    public override string DisplayName { get { return BattleData.Name; } }
    public Foe Visual { get; set; }
    public FoeBattleData BattleData { get; set; }
    public int MaxProblemJuice { get; set; }
    public int CurProblemJuice { get; set; }

    public int CurPhase { get; set; } = 0;
    public bool Standing { get; set; } = true;

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

    public void Progress(int amount)
    {
        CurProblemJuice = Mathf.Clamp(CurProblemJuice - amount, 0, MaxProblemJuice);
        Visual.HealthSlider.value = CurProblemJuice;
    }

    public override bool CanAct()
    {
        bool canAct = base.CanAct();
        canAct &= CurProblemJuice > 0;
        return canAct;
    }
}
