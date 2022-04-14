using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResolveState : IGameplayState
{
    public const float WaitAfterLoggingTargets = .2f;
    public const float WaitAfterLoggingEffect = .25f;
    public const float WaitAfterActionResolves = .5f;
    public const float WaitAfterProblemKO = .35f;

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
            if (command.ActingMember is FoeMember)
            {
                FoeMember foeMember = (FoeMember)command.ActingMember;
                if (foeMember.CurProblemJuice <= 0)
                {
                    Debug.Log("User was KO'd, skipping");
                    continue;
                }
            }

            // ConsoleManager.Instance.AddToLog($"Resolve this command from {command.ActingMember.DisplayName} targeting {command.Target.DisplayName} with {command.ActionTaken.MoveName}");

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
