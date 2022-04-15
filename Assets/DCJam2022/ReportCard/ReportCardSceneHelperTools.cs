using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReportCardSceneHelperTools : SceneHelperTools
{
    public Button RestartButton;
    public TMP_Text SupervisorStatement;
    public TMP_Text ClearText;

    public AudioClip ReportCardMusic;

    public List<string> MajorProblems = new List<string>();
    public List<string> MinorProblems = new List<string>();

    public override IGameplayState GetNewDemoState()
    {
        AudioManager.Instance?.PlayMusic(ReportCardMusic);
        return new ReportCardState();
    }
}
