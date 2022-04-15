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

        int majorSolved = 0, minorSolved = 0;

        foreach (string majorProblem in helperTools.MajorProblems)
        {
            if (SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData.GetFlag(majorProblem) > 0)
            {
                majorSolved++;
            }
        }

        foreach (string minorProblem in helperTools.MinorProblems)
        {
            if (SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData.GetFlag(minorProblem) > 0)
            {
                minorSolved++;
            }
        }

        float percentageOfMajor = helperTools.MajorProblems.Count;

        string solutionText = string.Format("(clearing text not set yet) You cleared {0}% of major obstacles and {1}% of minor obstacles in {2} days.", ((float)majorSolved / (float)helperTools.MajorProblems.Count).ToString(), ((float)minorSolved / (float)helperTools.MinorProblems.Count).ToString(), dayCount);

        helperTools.ClearText.text = solutionText;
        helperTools.SupervisorStatement.text = "This is the set supervisor statement text.";
    }

    void RestartGame()
    {
        SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData = null;
        SceneHelperInstance.PlayerParty = null;
        SceneHelperInstance.StartCoroutine(StateMachineInstance.ChangeToState(new InitialMainScreenState()));
    }
}
