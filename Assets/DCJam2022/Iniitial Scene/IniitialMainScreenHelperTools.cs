using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IniitialMainScreenHelperTools : SceneHelperTools
{
    public Button StartButton;
    public override IGameplayState GetNewDemoState()
    {
        return new InitialMainScreenState();
    }
}
