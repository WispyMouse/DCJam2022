using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportCardState : SceneLoadingGameplayState
{
    public override string SceneName => "ReportCard";

    ReportCardSceneHelperTools helperTools { get; set; }

    public override void SetControls(WarrencrawlInputs controls)
    {
        
    }

    public override void UnsetControls(WarrencrawlInputs controls)
    {
        
    }

    public override IEnumerator Load()
    {
        yield return base.Load();
        helperTools = GameObject.FindObjectOfType<ReportCardSceneHelperTools>();
        helperTools.RestartButton.onClick.AddListener(() => RestartGame());
    }

    void RestartGame()
    {
        SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData = null;
        SceneHelperInstance.PlayerParty = null;
        SceneHelperInstance.StartCoroutine(StateMachineInstance.ChangeToState(new InitialMainScreenState()));
    }
}
