using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialMainScreenState : SceneLoadingGameplayState
{
    public override string SceneName => "InitialScene";

    public override void SetControls(WarrencrawlInputs controls)
    {
        GameObject.FindObjectOfType<IniitialMainScreenHelperTools>().StartButton.onClick.AddListener(() => StartGame());
    }

    public override void UnsetControls(WarrencrawlInputs controls)
    {
        
    }

    void StartGame()
    {
        SceneHelperInstance.StartCoroutine(StateMachineInstance.ChangeToState(new PartySelectionState()));
    }
}
