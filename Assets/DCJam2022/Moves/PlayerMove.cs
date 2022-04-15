using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DCJam2022/PlayerMove")]
public class PlayerMove : MoveBase
{
    public bool SingleUsePerBattle;

    public bool UsedInThisBattle { get; set; }
}
