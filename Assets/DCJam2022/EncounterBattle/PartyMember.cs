using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : CombatMember
{
    public override string DisplayName => FromProfile.ProfileName;
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
        MaxNRG = fromProfile.MaxNRG;
    }

    public void RefreshOutOfBattle()
    {
        foreach (PlayerMove curMove in FromProfile.AttackOptions)
        {
            curMove.UsedInThisBattle = false;
        }
        CurNRG = MaxNRG;
    }

    public void LoseNRG(int amount)
    {
        CurNRG = Mathf.Clamp(CurNRG - amount, 0, MaxNRG);
        Hud.NRGSlider.SetValue(amount, MaxNRG);
    }
}
