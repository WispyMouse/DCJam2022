using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/CreateEncounterBattle")]
public class EncounterBattle : ScriptableObject
{
    public string EncounterName;
    public List<EncounterWave> Foes;
}
