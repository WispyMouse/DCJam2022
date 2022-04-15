using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NMEPreview : MoveTooltip
{
    public TMP_Text NMEName;

    public override void SetFromMove(MoveBase move)
    {
        base.SetFromMove(move);
        NMEName.text = GetComponentInParent<Foe>().DataMember.DisplayName;
    }
}
