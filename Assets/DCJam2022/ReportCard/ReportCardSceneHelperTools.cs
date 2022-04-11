using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportCardSceneHelperTools : SceneHelperTools
{
    public Button RestartButton;

    public override IGameplayState GetNewDemoState()
    {
        return new ReportCardState();
    }
}
