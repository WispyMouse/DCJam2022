using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents any character participating in a Battle.
/// </summary>
public abstract class CombatMember
{
    public abstract string DisplayName { get; }

    public virtual bool CanAct()
    {
        return true;
    }
}
