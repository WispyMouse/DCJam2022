using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTargetForPartyActionState : IGameplayState
{
    CombatMember choosingMember { get; set; }
    BattleState activeBattleState { get; set; }
    GlobalStateMachine stateMachineInstance { get; set; }
    PlayerMove forCommand { get; set; }

    public ChooseTargetForPartyActionState(GlobalStateMachine stateMachine, BattleState battleState, CombatMember forMember, PlayerMove command)
    {
        stateMachineInstance = stateMachine;
        activeBattleState = battleState;
        choosingMember = forMember;
        forCommand = command;
    }

    public IEnumerator Initialize()
    {
        yield break;
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
        foreach (FoeMember opponent in activeBattleState.Opponents.OpposingMembers)
        {
            opponent.Visual.ClearTargetable();
            opponent.Visual.SetUnhighlighted();
        }

        GameObject.FindObjectOfType<FoeSelectionHandler>().ChosenAction = null;

        yield break;
    }

    public IEnumerator ExitState(IGameplayState nextState)
    {
        foreach (FoeMember opponent in activeBattleState.Opponents.OpposingMembers)
        {
            opponent.Visual.ClearTargetable();
            opponent.Visual.SetUnhighlighted();
        }

        GameObject.FindObjectOfType<FoeSelectionHandler>().ChosenAction = null;

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
        foreach (PartyMember partyMember in activeBattleState.PlayerPartyPointer.PartyMembers)
        {
            if (partyMember == choosingMember)
            {
                if (forCommand.Targeting == Target.Self)
                {
                    activeBattleState.BattleCommands.Add(new BattleCommand(choosingMember, choosingMember, forCommand));
                    yield return stateMachine.EndCurrentState();
                    yield break;
                }

                if (forCommand.Targeting == Target.AllOpposing)
                {
                    activeBattleState.BattleCommands.Add(new BattleCommand(choosingMember, null, forCommand));
                    yield return stateMachine.EndCurrentState();
                    yield break;
                }

                partyMember.Hud.SetChooseTargets(forCommand, ActionCanceled);
            }
            else
            {
                partyMember.Hud.SetFade();
            }
        }

        foreach (FoeMember opponent in activeBattleState.Opponents.OpposingMembers)
        {
            opponent.Visual.SetTargetable(TargetChosen);
        }

        GameObject.FindObjectOfType<FoeSelectionHandler>().ChosenAction = TargetChosen;

        yield break;
    }

    public void UnsetControls(WarrencrawlInputs activeInput)
    {
        
    }

    void TargetChosen(CombatMember target)
    {
        Debug.Log("Target chosen");
        activeBattleState.BattleCommands.Add(new BattleCommand(choosingMember, target, forCommand));
        activeBattleState.SceneHelperInstance.StartCoroutine(stateMachineInstance.EndCurrentState());
    }

    void ActionCanceled(CombatMember member)
    {
        Debug.Log("Action canceled");
        activeBattleState.SceneHelperInstance.StartCoroutine(stateMachineInstance.EndCurrentState());
    }
}
