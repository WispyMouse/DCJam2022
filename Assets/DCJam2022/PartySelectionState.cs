using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySelectionState : SceneLoadingGameplayState
{
    public override string SceneName => "PartySelection";
    PartySelectionSceneHelperTools HelperTools { get; set; }

    List<DelverProfile> SelectedDelvers { get; set; } = new List<DelverProfile>();

    GlobalStateMachine stateMachine { get; set; }


    public override void SetControls(WarrencrawlInputs controls)
    {
        
    }

    public override void UnsetControls(WarrencrawlInputs controls)
    {
        
    }

    public override IEnumerator Load()
    {
        HelperTools = GameObject.FindObjectOfType<PartySelectionSceneHelperTools>();
        HelperTools.ClearButtonPressed.AddListener(() => { ClearButtonClicked(); });
        HelperTools.DelveButtonPressed.AddListener(() => DelveButtonClicked());
        HelperTools.DelverSelected.AddListener((DelverProfile delver) => { SelectDelver(delver); });
        HelperTools.SetSelectedDelvers(SelectedDelvers);
        yield break;
    }

    public override IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        yield return base.StartState(globalStateMachine, previousState);
    }

    public void DelveButtonClicked()
    {
        HelperTools.SceneHelperInstance.PlayerParty = new PlayerParty(SelectedDelvers);
        HelperTools.StartCoroutine(stateMachine.ChangeToState(new ButtonsState()));
    }

    public void ClearButtonClicked()
    {
        SelectedDelvers.Clear();
        HelperTools.SetSelectedDelvers(SelectedDelvers);
    }

    public void SelectDelver(DelverProfile partyMember)
    {
        if (SelectedDelvers.Count >= 3)
        {
            return;
        }

        if (SelectedDelvers.Contains(partyMember))
        {
            return;
        }

        SelectedDelvers.Add(partyMember);

        HelperTools.SetSelectedDelvers(SelectedDelvers);
    }
}
