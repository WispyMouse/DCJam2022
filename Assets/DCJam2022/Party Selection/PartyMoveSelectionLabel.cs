using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartyMoveSelectionLabel : MonoBehaviour
{
    public TMP_Text AbilityName;
    public MoveTooltipShower TooltipShower;

    public void SetFromMove(PlayerMove move)
    {
        AbilityName.text = move.MoveName;
        TooltipShower.Tooltip.SetFromMove(move);
    }
}
