using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsState : SceneLoadingGameplayState
{
    public override string SceneName => "Buttons";

    public override void SetControls(WarrencrawlInputs controls)
    {
        controls.UI.Enable();
    }

    public override void UnsetControls(WarrencrawlInputs controls)
    {
        controls.UI.Disable();
    }

    public override IEnumerator ChangeUp(IGameplayState nextState)
    {
        foreach (GameObject rootObj in SceneManager.GetSceneByName(SceneName).GetRootGameObjects())
        {
            rootObj.SetActive(false);
        }

        yield break;
    }

    public override IEnumerator ExitState(IGameplayState nextState)
    {
        foreach (GameObject rootObj in SceneManager.GetSceneByName(SceneName).GetRootGameObjects())
        {
            rootObj.SetActive(false);
        }

        yield break;
    }
}
