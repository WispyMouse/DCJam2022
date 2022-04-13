using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleChoiceEntry
{
    public List<FlagCheckCondition> FlagsRequired = new List<FlagCheckCondition>();
    public string ChoiceName;
    public int GotoId;
}
