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
    public GameObject RetreatButton;
    public TMP_Text EncounterName;

    public IncomingPreview Preview;

    public List<FoeBattleData> DefaultFoes;

    public void EndBattle(bool isVictory = true)
    {
        BattleState.LastWasVictory = isVictory;
        SceneHelperInstance.StartCoroutine(EndUntilBelowBattle());
    }

    public IEnumerator EndUntilBelowBattle()
    {
        while (SceneHelper.GlobalStateMachineInstance.CurrentState is not BattleState)
        {
            yield return SceneHelper.GlobalStateMachineInstance.EndCurrentState(true);
        }
        yield return SceneHelper.GlobalStateMachineInstance.EndCurrentState();
    }

    public override IGameplayState GetNewDemoState()
    {
        BattleOpponents opponents = new BattleOpponents(DefaultFoes);

        return new BattleState(opponents);
    }

    public UnityEvent CommitButtonPressed;
    public UnityEvent RetreatButtonPressed;

    public enum CommitButtonState { NoneSet, SomeSet, AllSet, Hide }
    public void SetCommitButtonState(CommitButtonState state)
    {
        switch (state)
        {
            case CommitButtonState.Hide:
                CommitButton.gameObject.SetActive(false);
                RetreatButton.SetActive(false);
                break;
            default:
                CommitButton.gameObject.SetActive(true);
                RetreatButton.SetActive(true);
                break;
        }
    }

    public void OnCommitButton()
    {
        CommitButtonPressed.Invoke();
    }

    public void OnRetreatButton()
    {
        RetreatButtonPressed.Invoke();
    }
}
