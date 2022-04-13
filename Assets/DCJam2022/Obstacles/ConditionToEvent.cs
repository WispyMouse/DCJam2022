using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionToEvent
{
    public List<FlagCheckCondition> FlagsToCheck;
    public int EventIDToGoTo;
}
