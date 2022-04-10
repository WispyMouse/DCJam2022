using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleSceneHelperTools : SceneHelperTools
{
    public Transform[] FoePositions;
    public Foe FoePF;

    public Transform PlayerHealthHUDPosition;
    public PlayerHealthInCombat HudPF;

    public GameObject CommitButton;

    public void EndBattle()
    {
        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.EndCurrentState());
    }

    public override IGameplayState GetNewDemoState()
    {
        BattleOpponents opponents = BattleState.GetDemoOpponents();

        return new BattleState(opponents);
    }

    public UnityEvent CommitButtonPressed;

    public enum CommitButtonState { NoneSet, SomeSet, AllSet, Hide }
    public void SetCommitButtonState(CommitButtonState state)
    {
        switch (state)
        {
            case CommitButtonState.Hide:
                CommitButton.gameObject.SetActive(false);
                break;
            default:
                CommitButton.gameObject.SetActive(true);
                break;
        }
    }

    public void OnCommitButton()
    {
        CommitButtonPressed.Invoke();
    }
}
