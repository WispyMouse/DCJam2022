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

    public virtual IEnumerator TakeAction(BattleState battleState)
    {
        string targetText;
        List<CombatMember> targetsAffected = new List<CombatMember>();

        switch (ActionTaken.Targeting)
        {
            case global::Target.OneAlly:
            case global::Target.OneOpposing:
                targetText = $"{ActingMember.DisplayName} {ActionTaken.Verbs} {Target.DisplayName}";
                targetsAffected.Add(ActingMember);
                break;
            case global::Target.AllOpposing:
                if (ActingMember is PartyMember)
                {
                    targetText = $"{ActingMember.DisplayName} {ActionTaken.Verbs}";
                    targetsAffected.AddRange(battleState.Opponents.OpposingMembers);
                }
                else
                {
                    targetText = $"{ActingMember.DisplayName} {ActionTaken.Verbs} everyone";
                    targetsAffected.AddRange(battleState.PlayerPartyPointer.PartyMembers);
                }
                break;
            case global::Target.AoF:
                targetText = $"{ActingMember.DisplayName} {ActionTaken.Verbs} the party";
                break;
            default:
                Debug.LogError($"Unhandled target type: {ActionTaken.Targeting}");
                targetText = "???";
                break;

        }

        ConsoleManager.Instance.AddToLog(targetText);
        yield return new WaitForSeconds(ResolveState.WaitAfterLoggingTargets);

        foreach (CombatMember affectedMember in targetsAffected)
        {
            if (ActionTaken.DamageCeiling > 0)
            {
                int damageRoll = Random.Range(ActionTaken.DamageFloor, ActionTaken.DamageCeiling + 1);

                if (ActionTaken.IsHealing)
                {
                    ConsoleManager.Instance.AddToLog($"{affectedMember.DisplayName} is refreshed by {damageRoll}");
                    // ASSUMES ONLY PLAYERS CAN HEAL; temporary
                    ((PartyMember)affectedMember).LoseNRG(-damageRoll);
                    yield return new WaitForSeconds(ResolveState.WaitAfterActionResolves);
                }
                else
                {
                    if (affectedMember is FoeMember)
                    {
                        ConsoleManager.Instance.AddToLog($"{damageRoll} progress is made");
                        ((FoeMember)affectedMember).Progress(damageRoll);
                        yield return new WaitForSeconds(ResolveState.WaitAfterActionResolves);
                    }
                    else
                    {
                        PartyMember member = (PartyMember)affectedMember;

                        if (member.CurNRG == 0)
                        {
                            ConsoleManager.Instance.AddToLog($"The party loses {damageRoll} AoF!!!");
                            battleState.PlayerPartyPointer.LoseAOF(damageRoll);
                            yield return new WaitForSeconds(ResolveState.WaitAfterActionResolves);
                        }
                        else
                        {
                            ConsoleManager.Instance.AddToLog($"{affectedMember.DisplayName} loses {damageRoll} NRG");
                            ((PartyMember)affectedMember).LoseNRG(damageRoll);
                            yield return new WaitForSeconds(ResolveState.WaitAfterActionResolves);
                        }
                    }
                }
            }
        }

        if (ActionTaken.Targeting == global::Target.AoF)
        {
            int damageRoll = Random.Range(ActionTaken.DamageFloor, ActionTaken.DamageCeiling + 1);
            battleState.PlayerPartyPointer.LoseAOF(damageRoll);
            ConsoleManager.Instance.AddToLog($"The party loses {damageRoll} AoF!!!");
            yield return new WaitForSeconds(ResolveState.WaitAfterActionResolves);
        }
    }
}
