using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthInCombat : MonoBehaviour
{
    public TMP_Text PlayerNameLabel;
    public GameObject KOFadeScreen;
    public GameObject FadeScreen;
    public GameObject CommandsScreen;
    public GameObject ChooseTargetScreen;
    public GameObject ActionSelectedScreen;

    public Button AttackOption;
    public Transform AttackOptionParent;

    public CombatMember Player { get; private set; }

    public void SetPlayer(CombatMember member)
    {
        PlayerNameLabel.text = member.DisplayName;

        Player = member;
    }

    public void SetReady(System.Action<CombatMember,string> takeAction)
    {
        ClearOptions();
        CommandsScreen.SetActive(true);

        for (int ii = 0; ii < AttackOptionParent.childCount; ii++)
        {
            Destroy(AttackOptionParent.GetChild(ii).gameObject);
        }

        foreach (string option in Player.AttackOptions)
        {
            Button newButton = Instantiate(AttackOption, AttackOptionParent);

            string optionHolder = option;
            newButton.GetComponentInChildren<TMP_Text>().text = optionHolder;
            newButton.onClick.AddListener(() => { takeAction(Player, optionHolder); });
        }
    }

    public void SetChooseTargets(string action, Action<CombatMember> cancelAction)
    {
        ClearOptions();
        ChooseTargetScreen.SetActive(true);

        Button cancelButton = ChooseTargetScreen.GetComponentInChildren<Button>();
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => cancelAction(Player));
    }

    public void SetCommand(BattleCommand toCommand, Action<CombatMember> cancelAction)
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

    public void SetKO()
    {
        ClearOptions();
        CommandsScreen.SetActive(true);
    }

    void ClearOptions()
    {
        CommandsScreen.SetActive(false);
        KOFadeScreen.SetActive(false);
        FadeScreen.SetActive(false);
        ChooseTargetScreen.SetActive(false);
        ActionSelectedScreen.SetActive(false);
    }
}
