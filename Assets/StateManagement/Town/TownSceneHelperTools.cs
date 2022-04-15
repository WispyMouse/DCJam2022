using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TownSceneHelperTools : SceneHelperTools
{
    public TMP_Text DelveButtonLabel;

    public void ToTheLabyrinth()
    {
        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.ChangeToState(new LabyrinthState(new LabyrinthSceneHelperGrabber())));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new TownState();
    }

    public void FinishGame()
    {
        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.ChangeToState(new ReportCardState()));
    }
}
