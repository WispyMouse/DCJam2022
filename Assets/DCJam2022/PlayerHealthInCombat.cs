using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthInCombat : MonoBehaviour
{
    public TMP_Text PlayerNameLabel;
    public GameObject DangerNote;
    public GameObject FadeScreen;
    public GameObject CommandsScreen;
    public GameObject ChooseTargetScreen;
    public GameObject ActionSelectedScreen;

    public MoveInEncounter AttackOption;
    public Transform AttackOptionParent;

    public Slider NRGSlider;
    public TMP_Text NRGLabel;

    public PartyMember Player { get; private set; }

    public void SetPlayer(PartyMember member)
    {
        PlayerNameLabel.text = member.DisplayName;

        Player = member;
        UpdateFromPlayer();
    }

    public void UpdateFromPlayer()
    {
        NRGSlider.maxValue = Player.MaxNRG;
        NRGSlider.value = Player.CurNRG;
        NRGLabel.text = Player.CurNRG.ToString();

        if (Player.CurNRG <= 0)
        {
            SetDanger();
        }
    }

    public void SetReady(System.Action<PartyMember, PlayerMove> takeAction)
    {
        ClearOptions();
        CommandsScreen.SetActive(true);

        for (int ii = 0; ii < AttackOptionParent.childCount; ii++)
        {
            Destroy(AttackOptionParent.GetChild(ii).gameObject);
        }

        foreach (PlayerMove option in Player.FromProfile.AttackOptions)
        {
            MoveInEncounter newButton = Instantiate(AttackOption, AttackOptionParent);

            PlayerMove optionHolder = option;
            newButton.SetFromMove(Player, optionHolder, takeAction);
        }

        UpdateFromPlayer();
    }

    public void SetChooseTargets(string action, Action<PartyMember> cancelAction)
    {
        ClearOptions();
        ChooseTargetScreen.SetActive(true);

        Button cancelButton = ChooseTargetScreen.GetComponentInChildren<Button>();
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => cancelAction(Player));
    }

    public void SetCommand(BattleCommand toCommand, Action<PartyMember> cancelAction)
    {
        ClearOptions();
        ActionSelectedScreen.SetActive(true);

        Button cancelButton = ActionSelectedScreen.GetComponentInChildren<Button>();
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => cancelAction(Player));
    }

    public void SetFade()
    {
        ClearOptions();
        FadeScreen.SetActive(true);
    }

    public void SetDanger()
    {
        DangerNote.SetActive(true);
    }

    void ClearOptions()
    {
        CommandsScreen.SetActive(false);
        DangerNote.SetActive(false);
        FadeScreen.SetActive(false);
        ChooseTargetScreen.SetActive(false);
        ActionSelectedScreen.SetActive(false);
    }
}
