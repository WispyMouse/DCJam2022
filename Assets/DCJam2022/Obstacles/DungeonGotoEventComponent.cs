using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/EventComponents/DungeonGotoEvent")]
public class DungeonGotoEventComponent : ObstacleEventComponent
{
    public override bool CloseCurrentState => true;
    public override IGameplayState GetNewState(SaveData activeSaveData)
    {
        return new LabyrinthState(new LabyrinthSceneHelperGrabber());
    }

    public override int AfterStateSetPointer(SaveData activeSaveData)
    {
        return -1;
    }
}
