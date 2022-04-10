using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerParty
{
    public List<PartyMember> PartyMembers { get; private set; } = new List<PartyMember>();

    public int MaxAOF { get; set; }
    public int CurAOF { get; set; }

    public PlayerParty()
    {
    }

    public PlayerParty(List<DelverProfile> profiles)
    {
        PartyMembers = new List<PartyMember>();

        foreach (DelverProfile profile in profiles)
        {
            AddPartyMember(profile);
        }
    }

    public void AddPartyMember(DelverProfile profile)
    {
        PartyMember member = new PartyMember(profile);
        AddPartyMember(member);
    }

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
