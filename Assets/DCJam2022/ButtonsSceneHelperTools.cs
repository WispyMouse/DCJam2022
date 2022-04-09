using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsSceneHelperTools : SceneHelperTools
{
    public override IGameplayState GetNewDemoState()
    {
        return new ButtonsState();
    }

    public void StartEncounter(int encounterId)
    {
        Debug.Log($"Begin encounterId {encounterId}");
        BattleOpponents opponents = new BattleOpponents();
        opponents.AddOpposingMember(new CombatMember() { DisplayName = "A" });
        opponents.AddOpposingMember(new CombatMember() { DisplayName = "B" });
        opponents.AddOpposingMember(new CombatMember() { DisplayName = "C" });

        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.ChangeToState(new BattleState(opponents)));
    }
}
