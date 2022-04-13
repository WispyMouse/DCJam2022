using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOpponents
{
    public List<FoeMember> OpposingMembers { get; private set; } = new List<FoeMember>();

    public BattleOpponents()
    {

    }

    public BattleOpponents(List<FoeBattleData> battleData)
    {
        foreach (FoeBattleData foe in battleData)
        {
            FoeMember thisMember = new FoeMember(new FoeEncounterPhase() { EncounteredFoe = foe });
            AddOpposingMember(thisMember);
        }
    }

    public void AddOpposingMember(FoeMember toAdd)
    {
        OpposingMembers.Add(toAdd);
    }
}
