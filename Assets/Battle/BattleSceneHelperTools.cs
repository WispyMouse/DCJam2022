using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneHelperTools : SceneHelperTools
{
    public Transform[] FoePositions;
    public Foe FoePF;

    public Transform PlayerHealthHUDPosition;
    public PlayerHealthInCombat HudPF;

    public void EndBattle()
    {
        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.EndCurrentState());
    }

    public override IGameplayState GetNewDemoState()
    {
        BattleOpponents opponents = new BattleOpponents();

        opponents.AddOpposingMember(new CombatMember() { DisplayName = "Dirty Dishes A" });
        opponents.AddOpposingMember(new CombatMember() { DisplayName = "Dirty Dishes B" });
        opponents.AddOpposingMember(new CombatMember() { DisplayName = "Dirty Dishes C" });

        opponents.AddOpposingMember(new CombatMember() { DisplayName = "Unopened Mail A" });
        opponents.AddOpposingMember(new CombatMember() { DisplayName = "Mailbox A" });
        opponents.AddOpposingMember(new CombatMember() { DisplayName = "Unopened Mail B" });

        return new BattleState(opponents);
    }
}
