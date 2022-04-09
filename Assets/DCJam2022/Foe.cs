using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : MonoBehaviour
{
    public CombatMember DataMember { get; protected set; }

    public void SetDataMember(CombatMember member)
    {
        DataMember = member;
    }
}
