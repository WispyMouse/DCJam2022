using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveState : IGameplayState
{
    public const float WaitAfterLoggingTargets = .4f;
    public const float WaitAfterLoggingEffect = .2f;
    public const float WaitAfterActionResolves = .25f;

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

            yield return command.TakeAction(managedBattleState);
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
