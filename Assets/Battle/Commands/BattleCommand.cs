using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCommand
{
    public CombatMember ActingMember { get; set; }
    public CombatMember Target { get; set; }
    public string ActionTaken { get; set; }

    public BattleCommand(CombatMember actor, CombatMember target, string actionTaken)
    {
        this.ActingMember = actor;
        this.Target = target;
        this.ActionTaken = actionTaken;
    }
}
