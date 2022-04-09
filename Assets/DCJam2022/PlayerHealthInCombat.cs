using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthInCombat : MonoBehaviour
{
    public TMP_Text PlayerNameLabel;
    public CombatMember Player { get; private set; }

    public void SetPlayer(CombatMember member)
    {
        PlayerNameLabel.text = member.DisplayName;

        Player = member;
    }
}
