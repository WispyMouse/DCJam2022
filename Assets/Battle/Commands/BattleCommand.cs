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
        if (!ActingMember.CanAct())
        {
            yield break;
        }

        string targetText;
        List<CombatMember> targetsAffected = new List<CombatMember>();

        switch (ActionTaken.Targeting)
        {
            case global::Target.Self:
                targetText = $"{ActingMember.DisplayName} {ActionTaken.Verbs}";
                targetsAffected.Add(ActingMember);
                break;
            case global::Target.OneAlly:
            case global::Target.OneOpposing:
                targetText = $"{ActingMember.DisplayName} {ActionTaken.Verbs} {Target.DisplayName}";
                targetsAffected.Add(Target);
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
                    if (affectedMember is FoeMember)
                    {
                        ConsoleManager.Instance.AddToLog($"{damageRoll} more hitpoints");
                        ((FoeMember)affectedMember).Progress(damageRoll);
                        battleState.UpdateEveryonesVisuals();
                        yield return new WaitForSeconds(ResolveState.WaitAfterLoggingEffect);
                    }
                    else
                    {
                        PartyMember member = (PartyMember)affectedMember;
                        ConsoleManager.Instance.AddToLog($"{affectedMember.DisplayName} is refreshed by {damageRoll}");
                        member.LoseNRG(-damageRoll);
                        battleState.UpdateEveryonesVisuals();
                        yield return new WaitForSeconds(ResolveState.WaitAfterLoggingEffect);
                    }
                }
                else
                {
                    if (affectedMember is FoeMember)
                    {
                        FoeMember member = (FoeMember)affectedMember;
                        if (member.CurProblemJuice <= 0)
                        {
                            continue;
                        }

                        ConsoleManager.Instance.AddToLog($"{damageRoll} progress is made");
                        member.Progress(damageRoll);
                        battleState.UpdateEveryonesVisuals();
                        yield return new WaitForSeconds(ResolveState.WaitAfterLoggingEffect);
                    }
                    else
                    {
                        PartyMember member = (PartyMember)affectedMember;

                        if (member.CurNRG == 0)
                        {
                            ConsoleManager.Instance.AddToLog($"The party loses {damageRoll} AoF!!!");
                            battleState.PlayerPartyPointer.LoseAOF(damageRoll);
                            battleState.UpdateEveryonesVisuals();
                            yield return new WaitForSeconds(ResolveState.WaitAfterLoggingEffect);
                        }
                        else
                        {
                            ConsoleManager.Instance.AddToLog($"{affectedMember.DisplayName} loses {damageRoll} NRG");
                            ((PartyMember)affectedMember).LoseNRG(damageRoll);
                            battleState.UpdateEveryonesVisuals();
                            yield return new WaitForSeconds(ResolveState.WaitAfterLoggingEffect);
                        }
                    }
                }
            }
        }

        if (ActionTaken.Targeting == global::Target.AoF)
        {
            int damageRoll = Random.Range(ActionTaken.DamageFloor, ActionTaken.DamageCeiling + 1);
            battleState.PlayerPartyPointer.LoseAOF(damageRoll);
            battleState.UpdateEveryonesVisuals();
            ConsoleManager.Instance.AddToLog($"The party loses {damageRoll} AoF!!!");
            yield return new WaitForSeconds(ResolveState.WaitAfterActionResolves);
        }
    }
}
