using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerParty
{
    public List<PartyMember> PartyMembers { get; private set; } = new List<PartyMember>();

    public void AddPartyMember(PartyMember toAdd)
    {
        PartyMembers.Add(toAdd);
        toAdd.RefreshOutOfBattle();
    }

    public PartyMember GetRandom()
    {
        if (!PartyMembers.Any())
        {
            return null;
        }

        int randomIndex = Random.Range(0, PartyMembers.Count);
        return PartyMembers[randomIndex];
    }
}
