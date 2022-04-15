using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IniitialMainScreenHelperTools : SceneHelperTools
{
    public Button StartButton;
    public AudioClip InitialScreenMusic;

    protected override IEnumerator StartChild()
    {
        AudioManager.Instance.PlayMusic(InitialScreenMusic);
        yield return base.StartChild();
    }
    public override IGameplayState GetNewDemoState()
    {
        return new InitialMainScreenState();
    }
}
