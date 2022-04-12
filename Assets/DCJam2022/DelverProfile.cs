using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/DelverProfile")]
public class DelverProfile : ScriptableObject
{
    public string ProfileName;
    public Sprite ChooseAPartyMemberPicture;
    public string Blurb;
    public int MaxNRG;

    public int Precision;
    public int Perception;
    public int Perseverence;

    public string FirstSkill;
    public string SecondSkill;
    public string ThirdSkill;

    public List<PlayerMove> AttackOptions;
}
