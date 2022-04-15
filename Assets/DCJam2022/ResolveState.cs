using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResolveState : IGameplayState
{
    public const float WaitAfterLoggingTargets = .15f;
    public const float WaitAfterLoggingEffect = .35f;
    public const float WaitAfterActionResolves = .6f;
    public const float WaitAfterProblemKO = .45f;

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
        managedBattleState.BattleCommands = managedBattleState.BattleCommands.OrderByDescending(bc => (int)bc.ActionTaken.Speed).ThenBy(bc => bc.ActingMember is PartyMember).ToList();

        foreach (BattleCommand command in managedBattleState.BattleCommands)
        {
            if (managedBattleState.PlayerPartyPointer.CurAOF <= 0)
            {
                break;
            }

            if (managedBattleState.Opponents.OpposingMembers.TrueForAll(x => x.CurProblemJuice <= 0))
            {
                break;
            }

            if (command.ActingMember is FoeMember)
            {
                FoeMember foeMember = (FoeMember)command.ActingMember;
                if (foeMember.CurProblemJuice <= 0)
                {
                    Debug.Log("User was KO'd, skipping");
                    continue;
                }
            }

            yield return command.TakeAction(managedBattleState);
            managedBattleState.UpdateEveryonesVisuals();
            yield return new WaitForSeconds(WaitAfterActionResolves);

            foreach (FoeMember curFoe in managedBattleState.Opponents.OpposingMembers)
            {
                if (curFoe.CurProblemJuice <= 0 && curFoe.Standing)
                {
                    ConsoleManager.Instance.AddToLog($"{curFoe.DisplayName} has been resolved!");
                    curFoe.Visual.SetKO();
                    managedBattleState.UpdateEveryonesVisuals();
                    yield return new WaitForSeconds(WaitAfterProblemKO);
                }
            }
        }

        foreach (FoeMember foe in managedBattleState.Opponents.OpposingMembers)
        {
            foe.GoNextPhase();
        }

        managedBattleState.UpdateEveryonesVisuals();

        yield return stateMachine.EndCurrentState();
    }

    public void UnsetControls(WarrencrawlInputs activeInput)
    {
        
    }
}
