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


    public GameObject CommitButton;
    public EncounterBattle Encounter { get; set; }

    int CurWave { get; set; } = 0;
    bool FirstWaveSpawned { get; set; } = false;

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

    public BattleState(EncounterBattle encounter)
    {
        Encounter = encounter;
        Opponents = new BattleOpponents();

        foreach (FoeEncounterPhase foe in encounter.Foes[CurWave].FoesInWave)
        {
            Opponents.AddOpposingMember(new FoeMember(foe));
        }
    }

    public override IEnumerator Initialize()
    {
        PlayerPartyPointer = SceneHelperInstance.PlayerParty;

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

        yield break;
    }

    public override IEnumerator Load()
    {
        yield return base.Load();

        BattleSceneHelperToolsInstance = GameObject.FindObjectOfType<BattleSceneHelperTools>();
    }

    public override IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        yield return base.StartState(globalStateMachine, previousState);

        BattleCommands = new List<BattleCommand>();
        Debug.Log("The stack is being emptied to here, new battlecommands");
        AOFBar.Instance.SetValue(PlayerPartyPointer.CurAOF, PlayerPartyPointer.MaxAOF);

        if (Encounter != null)
        {
            BattleSceneHelperToolsInstance.EncounterName.text = Encounter.EncounterName;
        }

        if (!FirstWaveSpawned)
        {
            FirstWaveSpawned = true;
            yield return SpawnCurrentWaveAndContinueFight();
        }
        else if (Opponents.OpposingMembers.All(op => op.CurProblemJuice <= 0))
        {
            yield return ProceedNextWave();
        }
        else
        {
            yield return globalStateMachine.PushNewState(new ChoosePartysCommandsState(globalStateMachine, this));
        }
    }

    public IEnumerator ProceedNextWave()
    {
        CurWave++;

        if (CurWave >= Encounter.Foes.Count)
        {
            Debug.Log("You win!");
            SceneHelperInstance.StartCoroutine(StateMachineInstance.EndCurrentState());
        }
        else
        {
            yield return SpawnCurrentWaveAndContinueFight();
        }
    }

    public IEnumerator SpawnCurrentWaveAndContinueFight()
    {
        foreach (Transform curFoePosition in BattleSceneHelperToolsInstance.FoePositions)
        {
            for (int ii = 0; ii < curFoePosition.childCount; ii++)
            {
                GameObject.Destroy(curFoePosition.GetChild(ii).gameObject);
            }
        }

        if (Encounter != null)
        {
            BattleOpponents curOpponents = new BattleOpponents();

            foreach (FoeEncounterPhase foe in Encounter.Foes[CurWave].FoesInWave)
            {
                curOpponents.AddOpposingMember(new FoeMember(foe));
            }

            Opponents = curOpponents;
        }

        for (int ii = 0; ii < BattleSceneHelperToolsInstance.FoePositions.Length && ii < Opponents.OpposingMembers.Count; ii++)
        {
            Foe foe = GameObject.Instantiate(BattleSceneHelperToolsInstance.FoePF, BattleSceneHelperToolsInstance.FoePositions[ii]);
            Opponents.OpposingMembers[ii].Visual = foe;
            foe.SetDataMember(Opponents.OpposingMembers[ii]);
        }

        yield return StateMachineInstance.PushNewState(new ChoosePartysCommandsState(StateMachineInstance, this));
    }
}
