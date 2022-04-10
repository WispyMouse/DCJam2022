using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/ObstacleEvent")]
public class ObstacleEvent : ScriptableObject
{
    public List<ObstacleEventComponent> EventComponents = new List<ObstacleEventComponent>();
    public string ObstacleName;
}
