using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartySelectionSceneHelperTools : SceneHelperTools
{
    public GameObject DelveButton;

    public UnityEvent DelveButtonPressed;
    public UnityEvent ClearButtonPressed;
    public UnityEvent<DelverProfile> DelverSelected;

    public Transform SelectedDelversRoot;

    public override IGameplayState GetNewDemoState()
    {
        return new PartySelectionState();
    }

    public void SelectDelver(DelverProfile profile)
    {
        DelverSelected.Invoke(profile);
    }

    public void OnDelveTheDepths()
    {
        DelveButtonPressed.Invoke();
    }

    public void OnClearButton()
    {
        ClearButtonPressed.Invoke();
    }

    public void SetSelectedDelvers(List<DelverProfile> selected)
    {
        for (int ii = 0; ii < 3; ii++)
        {
            if (selected.Count > ii)
            {
                DelverProfile profile = selected[ii];
                SelectedDelversRoot.GetChild(ii).gameObject.SetActive(true);
                SelectedDelversRoot.GetChild(ii).gameObject.GetComponent<SelectedDelverBottom>().SetDelver(profile);
            }
            else
            {
                SelectedDelversRoot.GetChild(ii).gameObject.SetActive(false);
            }
        }

        DelveButton.SetActive(selected.Count > 0);
    }
}
