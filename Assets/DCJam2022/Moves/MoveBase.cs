using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class MoveBase : ScriptableObject
{
    public string MoveName;
    [Tooltip("Alice [Verbs] Bob")]
    public string Verbs;

    public Target Targeting;
    public AppliedStatus StatusEffect;
    public int DamageFloor;
    public int DamageCeiling;
    public string Description;
    public bool IsHealing;
    public SpeedTier Speed;
}
