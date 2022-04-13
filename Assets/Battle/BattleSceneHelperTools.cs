using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class BattleSceneHelperTools : SceneHelperTools
{
    public Transform[] FoePositions;
    public Foe FoePF;

    public Transform PlayerHealthHUDPosition;
    public PlayerHealthInCombat HudPF;

    public GameObject CommitButton;
    public TMP_Text EncounterName;

    public List<FoeBattleData> DefaultFoes;

    public void EndBattle()
    {
        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.EndCurrentState());
    }

    public override IGameplayState GetNewDemoState()
    {
        BattleOpponents opponents = new BattleOpponents(DefaultFoes);

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
