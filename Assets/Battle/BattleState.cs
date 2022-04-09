using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents the state of conflict between the player's party and another, usually of monsters of somesort.
/// </summary>
public class BattleState : SceneLoadingGameplayState
{
    public override string SceneName => "Battle";

    /// <summary>
    /// A reference to the current party. There should be one of these during gameplay, and when most of this script runs.
    /// </summary>
    public PlayerParty PlayerPartyPointer { get; set; }

    /// <summary>
    /// A reference to an opposing party for this conflict. Passed in the constructor or built during it.
    /// </summary>
    public BattleOpponents Opponents { get; set; }

    /// <summary>
    /// A list of commands to execute gameplay with.
    /// Set by the level above it, <see cref="ChooseCommandsForPartyState"/>, to be executed in <see cref="ResolveCommandsState"/>.
    /// </summary>
    public List<BattleCommand> BattleCommands { get; set; } = new List<BattleCommand>();

    BattleSceneHelperTools BattleSceneHelperToolsInstance { get; set; }

    public BattleOpponents LastLoadedOpponents { get; set; }

    public override void SetControls(WarrencrawlInputs controls)
    {
        // TODO: Enable battle controls
    }

    public override void UnsetControls(WarrencrawlInputs controls)
    {
        
    }

    public BattleState()
    {
        Opponents = new BattleOpponents();
    }

    public BattleState(BattleOpponents opponents)
    {
        Opponents = opponents;
    }

    public override IEnumerator Load()
    {
        yield return base.Load();

        BattleSceneHelperToolsInstance = GameObject.FindObjectOfType<BattleSceneHelperTools>();

        PlayerPartyPointer = SceneHelperInstance.PlayerParty;

        foreach (Transform curFoePosition in BattleSceneHelperToolsInstance.FoePositions)
        {
            for (int ii = 0; ii < curFoePosition.childCount; ii++)
            {
                GameObject.Destroy(curFoePosition.GetChild(ii).gameObject);
            }
        }

        for (int ii = 0; ii < BattleSceneHelperToolsInstance.FoePositions.Length && ii < Opponents.OpposingMembers.Count; ii++)
        {
            Foe foe = GameObject.Instantiate(BattleSceneHelperToolsInstance.FoePF, BattleSceneHelperToolsInstance.FoePositions[ii]);
            Opponents.OpposingMembers[ii].Visual = foe;
            foe.SetDataMember(Opponents.OpposingMembers[ii]);
        }

        for (int ii = 0; ii < BattleSceneHelperToolsInstance.PlayerHealthHUDPosition.childCount; ii++)
        {
            GameObject.Destroy(BattleSceneHelperToolsInstance.PlayerHealthHUDPosition.GetChild(ii).gameObject);
        }

        for (int ii = 0; ii < PlayerPartyPointer.PartyMembers.Count; ii++)
        {
            PlayerHealthInCombat player = GameObject.Instantiate(BattleSceneHelperToolsInstance.HudPF, BattleSceneHelperToolsInstance.PlayerHealthHUDPosition);
            player.SetPlayer(PlayerPartyPointer.PartyMembers[ii]);
            PlayerPartyPointer.PartyMembers[ii].Hud = player;
        }
    }

    public override IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        if (previousState is ChooseCommandsForPartyState && BattleCommands.Any())
        {
            foreach (CombatMember opponent in Opponents.OpposingMembers)
            {
                int randomTarget = Random.Range(0, PlayerPartyPointer.PartyMembers.Count);

                BattleCommands.Add(new BattleCommand(opponent, PlayerPartyPointer.PartyMembers[randomTarget]));
            }

            yield return globalStateMachine.PushNewState(new ResolveCommandsState(this, BattleCommands));
        }
        else
        {
            BattleCommands = new List<BattleCommand>();
            yield return globalStateMachine.PushNewState(new ChoosePartysCommandsState(this));
        }
    }
}
