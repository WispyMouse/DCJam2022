using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
