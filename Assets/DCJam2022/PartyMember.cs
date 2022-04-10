using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : CombatMember
{
    public PlayerHealthInCombat Hud { get; set; }
    public List<string> AttackOptions { get; set; }
    public int MaxNRG { get; set; }
    public int CurNRG { get; set; }

    public void RefreshOutOfBattle()
    {
        CurNRG = MaxNRG;
    }
}
