using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PartySelectionSceneHelperTools : SceneHelperTools
{
    public GameObject DelveButton;

    public UnityEvent DelveButtonPressed;
    public UnityEvent ClearButtonPressed;
    public UnityEvent<DelverProfile> DelverSelected;

    public Image Portrait;
    public TMP_Text Name;
    public TMP_Text Blurb;
    public TMP_Text NRG;

    public TMP_Text FirstSkill;
    public TMP_Text SecondSkill;

    public Transform SelectedDelversRoot;

    DelverProfile lastHighlighted { get; set; }

    private void Awake()
    {
        HoveredDungeoneer(null);
    }

    public override IGameplayState GetNewDemoState()
    {
        return new PartySelectionState();
    }

    public void HoveredDungeoneer(DelverProfile profile)
    {
        lastHighlighted = profile;

        if (profile == null)
        {
            Portrait.gameObject.SetActive(false);
            Name.gameObject.SetActive(false);
            NRG.gameObject.SetActive(false);
            Blurb.gameObject.SetActive(false);

            FirstSkill.gameObject.SetActive(false);
            SecondSkill.gameObject.SetActive(false);
        }
        else 
        {
            Portrait.gameObject.SetActive(true);
            Name.gameObject.SetActive(true);
            NRG.gameObject.SetActive(true);
            Blurb.gameObject.SetActive(true);

            Portrait.sprite = profile.ChooseAPartyMemberPicture;
            Name.text = profile.ProfileName;
            NRG.text = $"NRG: {profile.MaxNRG}";
            Blurb.text = profile.Blurb;

            if (!string.IsNullOrEmpty(profile.FirstSkill))
            {
                FirstSkill.text = profile.FirstSkill;
                FirstSkill.gameObject.SetActive(true);
            }
            else
            {
                FirstSkill.gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(profile.SecondSkill))
            {
                SecondSkill.text = profile.SecondSkill;
                SecondSkill.gameObject.SetActive(true);
            }
            else
            {
                SecondSkill.gameObject.SetActive(false);
            }
        }
    }

    public void ClearHoveredDungeoneer(DelverProfile profile)
    {
        if (lastHighlighted == profile)
        {
            HoveredDungeoneer(null);
        }
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
