using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : CombatMember
{
    public PlayerHealthInCombat Hud { get; set; }
    public int MaxNRG { get; set; }
    public int CurNRG { get; set; }

    public DelverProfile FromProfile { get; set; }

    public PartyMember()
    {

    }

    public PartyMember(DelverProfile fromProfile)
    {
        FromProfile = fromProfile;
        DisplayName = fromProfile.ProfileName;
        MaxNRG = fromProfile.MaxNRG;
    }

    public void RefreshOutOfBattle()
    {
        CurNRG = MaxNRG;
    }

    public void LoseNRG(int amount)
    {
        CurNRG = Mathf.Clamp(0, MaxNRG, CurNRG - amount);
    }
}
