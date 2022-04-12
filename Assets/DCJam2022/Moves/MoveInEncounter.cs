using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoveInEncounter : MonoBehaviour
{
    public TMP_Text ButtonLabel;
    public Button Clickable;
    public MoveTooltipShower Shower;

    public void SetFromMove(PartyMember attacker, PlayerMove toSetFrom, Action<PartyMember, PlayerMove> clickedAction)
    {
        ButtonLabel.text = toSetFrom.MoveName;
        Clickable.onClick.AddListener(() => { clickedAction(attacker, toSetFrom); });
        Shower.Tooltip.SetFromMove(toSetFrom);
    }
}
