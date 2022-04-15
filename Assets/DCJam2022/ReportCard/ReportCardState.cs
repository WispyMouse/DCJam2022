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

        int dayCount = helperTools.SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData.Day;

        helperTools.ClearText.text = string.Format($"(clearing text not set yet) You cleared {0}% of major obstacles and {1}% of minor obstacles in {2} days.", 0, 0, dayCount);
        helperTools.SupervisorStatement.text = "This is the set supervisor statement text.";
    }

    void RestartGame()
    {
        SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData = null;
        SceneHelperInstance.PlayerParty = null;
        SceneHelperInstance.StartCoroutine(StateMachineInstance.ChangeToState(new InitialMainScreenState()));
    }
}
