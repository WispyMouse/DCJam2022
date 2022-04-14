using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/EventComponents/FlagCheck")]
public class ObstacleFlagCheckComponent : ObstacleEventComponent
{
    public List<ConditionToEvent> Conditions;
    public int FallbackEventId;

    int chosenValue { get; set; }

    public override IGameplayState GetNewState(SaveData activeSaveData)
    {
        for (int ii = 0; ii < Conditions.Count; ii++)
        {
            bool passes = true;

            foreach (FlagCheckCondition condition in Conditions[ii].FlagsToCheck)
            {
                int value = activeSaveData.GetFlag(condition.FlagToCheck);

                if (value >= condition.RequiredMinValue)
                {
                    // we passed!
                }
                else
                {
                    passes = false;
                    break;
                }
            }

            if (passes)
            {
                chosenValue = Conditions[ii].EventIDToGoTo;
                return null;
            }
        }

        return null;
    }

    public override int AfterStateSetPointer(SaveData activeSaveData)
    {
        return chosenValue;
    }
}
