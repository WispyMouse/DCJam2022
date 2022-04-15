using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownState : SceneLoadingGameplayState
{
    public override string SceneName => "Town";

    public override void SetControls(WarrencrawlInputs controls)
    {
        controls.UI.Enable();
    }

    public override void UnsetControls(WarrencrawlInputs activeInput)
    {
        activeInput.UI.Disable();
    }

    public override IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        yield return base.StartState(globalStateMachine, previousState);

        // heal the party back to full when they visit town
        foreach (PartyMember member in SceneHelperInstance.PlayerParty.PartyMembers)
        {
            member.CurNRG = member.MaxNRG;

            foreach (PlayerMove curMove in member.FromProfile.AttackOptions)
            {
                curMove.UsedThisDay = false;
            }
        }

        SceneHelperInstance.PlayerParty.CurAOF = SceneHelperInstance.PlayerParty.MaxAOF;
    }
}
