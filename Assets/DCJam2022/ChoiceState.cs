using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceState : IGameplayState
{
    ChoiceHandler ChoiceHandlerInstance { get; set; }

    Action<int> IndexSetter { get; set; }
    ObstacleChoiceComponent Component { get; set; }

    public ChoiceState(Action<int> indexSetter, ObstacleChoiceComponent component)
    {
        IndexSetter = indexSetter;
        Component = component;
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
        ChoiceHandlerInstance.Dimmer.SetActive(false);
        yield break;
    }

    public IEnumerator ExitState(IGameplayState nextState)
    {
        ChoiceHandlerInstance.Dimmer.SetActive(false);
        yield break;
    }

    public IEnumerator Initialize()
    {
        yield break;
    }

    public IEnumerator Load()
    {
        ChoiceHandlerInstance = GameObject.FindObjectOfType<ChoiceHandler>();
        yield break;
    }

    public void SetControls(WarrencrawlInputs activeInput)
    {
    }

    public IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        ChoiceHandlerInstance.Dimmer.gameObject.SetActive(true);

        ChoiceHandlerInstance.DecisionText.text = Component.Text;

        for (int ii = 0; ii < ChoiceHandlerInstance.ChoiceParent.childCount; ii++)
        {
            GameObject.Destroy(ChoiceHandlerInstance.ChoiceParent.GetChild(ii).gameObject);
        }

        foreach (ObstacleChoiceEntry entry in Component.Entries)
        {
            ObstacleChoiceEntry entryHolder = entry;
            Button newButton = GameObject.Instantiate(ChoiceHandlerInstance.ChoicePF, ChoiceHandlerInstance.ChoiceParent);
            newButton.GetComponentInChildren<TMP_Text>().text = entry.ChoiceName;
            newButton.onClick.AddListener(() => { ChoiceSelected(entryHolder); });

            bool shouldShow = true;

            foreach (FlagCheckCondition check in entry.FlagsRequired)
            {
                if (ChoiceHandlerInstance.SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData.GetFlag(check.FlagToCheck) < check.RequiredMinValue)
                {
                    shouldShow = false;
                    break;
                }
            }

            newButton.interactable = shouldShow;
        }

        yield break;
    }

    public void UnsetControls(WarrencrawlInputs activeInput)
    {
    }

    void ChoiceSelected(ObstacleChoiceEntry entry)
    {
        IndexSetter(entry.GotoId);
        ChoiceHandlerInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.EndCurrentState());
    }
}
