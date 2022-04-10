using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSceneHelperTools : SceneHelperTools
{
    public List<ObstacleEvent> Events = new List<ObstacleEvent>();
    public Button ButtonPF;
    public Transform ButtonParent;

    protected override IEnumerator StartChild()
    {
        yield return base.StartChild();

        for (int ii = 0; ii < Events.Count; ii++)
        {
            ObstacleEvent encounter = Events[ii];
            Button newButton = Instantiate(ButtonPF, ButtonParent);
            newButton.GetComponentInChildren<TMP_Text>().text = encounter.ObstacleName;

            newButton.onClick.AddListener(() => StartEvent(encounter));
        }
    }

    public override IGameplayState GetNewDemoState()
    {
        return new ButtonsState();
    }

    public void StartEvent(ObstacleEvent eventToStart)
    {
        if (!(SceneHelper.GlobalStateMachineInstance.CurrentState is ButtonsState))
        {
            return;
        }

        Debug.Log($"Begin {eventToStart.ObstacleName}");
        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.PushNewState(new HandleObstacleState(eventToStart)));
    }
}
