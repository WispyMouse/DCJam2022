using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTargetForPartyActionState : IGameplayState
{
    CombatMember choosingMember { get; set; }
    BattleState activeBattleState { get; set; }
    GlobalStateMachine stateMachineInstance { get; set; }
    string forCommand { get; set; }

    public ChooseTargetForPartyActionState(GlobalStateMachine stateMachine, BattleState battleState, CombatMember forMember, string command)
    {
        stateMachineInstance = stateMachine;
        activeBattleState = battleState;
        choosingMember = forMember;
        forCommand = command;

        Debug.Log("New state pushed");
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
        foreach (CombatMember opponent in activeBattleState.Opponents.OpposingMembers)
        {
            opponent.Visual.ClearTargetable();
            opponent.Visual.SetUnhighlighted();
        }

        GameObject.FindObjectOfType<FoeSelectionHandler>().ChosenAction = null;

        yield break;
    }

    public IEnumerator ExitState(IGameplayState nextState)
    {
        Debug.Log("ChooseTarget ExitState");

        foreach (CombatMember opponent in activeBattleState.Opponents.OpposingMembers)
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
        foreach (CombatMember partyMember in activeBattleState.PlayerPartyPointer.PartyMembers)
        {
            if (partyMember == choosingMember)
            {
                partyMember.Hud.SetChooseTargets("foo", ActionCanceled);
            }
            else
            {
                partyMember.Hud.SetFade();
            }
        }

        foreach (CombatMember opponent in activeBattleState.Opponents.OpposingMembers)
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
        activeBattleState.BattleCommands.Add(new BattleCommand(choosingMember, null));
        activeBattleState.SceneHelperInstance.StartCoroutine(stateMachineInstance.EndCurrentState());
    }

    void ActionCanceled(CombatMember member)
    {
        Debug.Log("Action canceled");
        activeBattleState.SceneHelperInstance.StartCoroutine(stateMachineInstance.EndCurrentState());
    }
}
