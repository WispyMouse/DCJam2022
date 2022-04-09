using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePartysCommandsState : IGameplayState
{
    BattleState managedBattleState { get; set; }

    public ChoosePartysCommandsState(BattleState curState)
    {
        managedBattleState = curState;
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

    public IEnumerator Load()
    {
        yield break;
    }

    public void SetControls(WarrencrawlInputs activeInput)
    {
        
    }

    public IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        if (previousState is BattleState)
        {
            // set members to act ready mode
        }

        yield break;
    }

    public void UnsetControls(WarrencrawlInputs activeInput)
    {
        
    }
}
