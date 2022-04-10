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
            Debug.Log($"Resolve this command from {command.ActingMember.DisplayName} targeting {command.Target.DisplayName} with {command.ActionTaken}");
            
            if (command.ActingMember is FoeMember)
            {
                FoeMember foeMember = (FoeMember)command.ActingMember;
                if (foeMember.CurProblemJuice <= 0)
                {
                    Debug.Log("User was KO'd, skipping");
                    continue;
                }
            }

            if (command.Target is FoeMember)
            {
                FoeMember foeMember = (FoeMember)command.Target;
                foeMember.CurProblemJuice = Mathf.Max(0, foeMember.CurProblemJuice - Mathf.CeilToInt(Random.value * 20));
                foeMember.Visual.UpdateFromMember();
            }
            else
            {
                PartyMember partyMember = (PartyMember)command.Target;
                partyMember.CurNRG = Mathf.Max(0, partyMember.CurNRG - Mathf.CeilToInt(Random.value * 20));
                partyMember.Hud.UpdateFromPlayer();
            }

            yield return new WaitForSeconds(.2f);
        }

        yield return stateMachine.EndCurrentState();
    }

    public void UnsetControls(WarrencrawlInputs activeInput)
    {
        
    }
}
