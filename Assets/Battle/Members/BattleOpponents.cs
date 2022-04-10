using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOpponents
{
    public List<FoeMember> OpposingMembers { get; private set; } = new List<FoeMember>();

    public void AddOpposingMember(FoeMember toAdd)
    {
        OpposingMembers.Add(toAdd);
    }
}
