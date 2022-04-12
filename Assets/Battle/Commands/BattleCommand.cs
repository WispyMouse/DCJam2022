using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCommand
{
    public CombatMember ActingMember { get; set; }
    public CombatMember Target { get; set; }
    public MoveBase ActionTaken { get; set; }

    public BattleCommand(CombatMember actor, CombatMember target, MoveBase actionTaken)
    {
        this.ActingMember = actor;
        this.Target = target;
        this.ActionTaken = actionTaken;
    }
}
