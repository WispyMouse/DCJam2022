using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveState : IGameplayState
{
    BattleState managedBattleState { get; set; }
    GlobalStateMachine stateMachineInstance { get; set; }

    public ResolveState(GlobalStateMachine stateMachine, BattleState curState)
    {
        managedBattleState = curState;
        stateMachineInstance = stateMachine;
    }

    public IEnumerator AnimateTransitionIn(IGameplayState previousState)
    {
        yield break;
    }

    public IEnumerator AnimateTransitionOut(IGameplayState nextState)
    {
        yield break;
    }

    public IEnumerator ChangeUp(IGameplayState nextState)
    {
        yield break;
    }

    public IEnumerator ExitState(IGameplayState nextState)
    {
        yield break;
    }

    public IEnumerator Initialize()
    {
        yield break;
    }

    public IEnumerator Load()
    {
        yield break;
    }

    public void SetControls(WarrencrawlInputs activeInput)
    {
        
    }

    public IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        foreach (BattleCommand command in managedBattleState.BattleCommands)
        {
            if (command.ActingMember is FoeMember)
            {
                FoeMember foeMember = (FoeMember)command.ActingMember;
                if (foeMember.CurProblemJuice <= 0)
                {
                    Debug.Log("User was KO'd, skipping");
                    continue;
                }
            }

            ConsoleManager.Instance.AddToLog($"Resolve this command from {command.ActingMember.DisplayName} targeting {command.Target.DisplayName} with {command.ActionTaken}");

            if (command.Target is FoeMember)
            {
                FoeMember foeMember = (FoeMember)command.Target;
                int damage = Mathf.CeilToInt(Random.value * 5);
                ConsoleManager.Instance.AddToLog($"{foeMember.DisplayName} damaged for {damage}.");
                foeMember.CurProblemJuice = Mathf.Max(0, foeMember.CurProblemJuice - damage);

                if (foeMember.CurProblemJuice <= 0)
                {
                    ConsoleManager.Instance.AddToLog("Solved!");
                    yield return new WaitForSeconds(.2f);
                }

                foeMember.Visual.UpdateFromMember();

                yield return new WaitForSeconds(.2f);
            }
            else
            {
                PartyMember partyMember = (PartyMember)command.Target;

                int damage = Mathf.CeilToInt(Random.value * 5);

                if (partyMember.CurNRG <= 0)
                {
                    ConsoleManager.Instance.AddToLog($"AOF damaged for {damage}.");
                    managedBattleState.PlayerPartyPointer.CurAOF = Mathf.Max(0, managedBattleState.PlayerPartyPointer.CurAOF - damage);
                    AOFBar.Instance.SetValue(managedBattleState.PlayerPartyPointer.CurAOF, managedBattleState.PlayerPartyPointer.MaxAOF);
                }
                else
                {
                    ConsoleManager.Instance.AddToLog($"{partyMember.DisplayName} damaged for {damage}.");
                    partyMember.CurNRG = Mathf.Max(0, partyMember.CurNRG - damage);
                    partyMember.Hud.UpdateFromPlayer();
                }
            }

            yield return new WaitForSeconds(.4f);
        }

        foreach (FoeMember foe in managedBattleState.Opponents.OpposingMembers)
        {
            foe.GoNextPhase();
            foe.Visual.UpdateFromMember();
        }

        yield return stateMachine.EndCurrentState();
    }

    public void UnsetControls(WarrencrawlInputs activeInput)
    {
        
    }
}
