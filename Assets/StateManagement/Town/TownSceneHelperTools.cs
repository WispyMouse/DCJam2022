using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TownSceneHelperTools : SceneHelperTools
{
    public TMP_Text DelveButtonLabel;

    public void ToTheLabyrinth()
    {
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

        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.ChangeToState(new LabyrinthState(new LabyrinthSceneHelperGrabber())));
    }

    public override IGameplayState GetNewDemoState()
    {
        return new TownState();
    }

    public void FinishGame()
    {
        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.ChangeToState(new ReportCardState()));
    }
}
