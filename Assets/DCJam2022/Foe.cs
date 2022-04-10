using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : MonoBehaviour
{
    public CombatMember DataMember { get; protected set; }
    System.Action<CombatMember> actionOnSelection { get; set; }

    public GameObject HighlightArrow;

    public void SetDataMember(CombatMember member)
    {
        DataMember = member;
    }

    public void SetTargetable(System.Action<CombatMember> chosen)
    {
        actionOnSelection = chosen;
    }

    public void ClearTargetable()
    {

    }

    public void SetHighlighted()
    {
        HighlightArrow.SetActive(true);
    }

    public void SetUnhighlighted()
    {
        HighlightArrow.SetActive(false);
    }
}
