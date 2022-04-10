using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoosePartysCommandsState : IGameplayState
{
    BattleSceneHelperTools battleSceneHelperTools { get; set; }
    BattleState managedBattleState { get; set; }
    GlobalStateMachine stateMachineInstance { get; set; }

    public ChoosePartysCommandsState(GlobalStateMachine stateMachine, BattleState curState)
    {
        managedBattleState = curState;
        stateMachineInstance = stateMachine;
    }

    public IEnumerator AnimateTransitionIn(IGameplayState previousState)
    {
        yield break;
    }

    public IEnumerator AnimateTransitionOut(IGameplayState nextState)
    {
        yield break;
    }

    public IEnumerator ChangeUp(IGameplayState nextState)
    {
        battleSceneHelperTools.SetCommitButtonState(BattleSceneHelperTools.CommitButtonState.Hide);
        battleSceneHelperTools.CommitButtonPressed.RemoveAllListeners();
        yield break;
    }

    public IEnumerator ExitState(IGameplayState nextState)
    {
        Debug.Log("Choose Party Commands State Exit");
        battleSceneHelperTools.SetCommitButtonState(BattleSceneHelperTools.CommitButtonState.Hide);
        battleSceneHelperTools.CommitButtonPressed.RemoveAllListeners();
        yield break;
    }

    public IEnumerator Load()
    {
        battleSceneHelperTools = GameObject.FindObjectOfType<BattleSceneHelperTools>();
        yield break;
    }

    public IEnumerator Initialize()
    {
        yield break;
    }

    public void SetControls(WarrencrawlInputs activeInput)
    {
        
    }

    public IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        SetReadyState();
        
        battleSceneHelperTools.CommitButtonPressed.AddListener(() => GoToResolve());

        yield break;
    }

    public void UnsetControls(WarrencrawlInputs activeInput)
    {
        
    }

    public void CommandSelected(CombatMember forMember, string command)
    {
        Debug.Log("Command selected");
        managedBattleState.SceneHelperInstance.StartCoroutine(stateMachineInstance.PushNewState(new ChooseTargetForPartyActionState(stateMachineInstance, managedBattleState, forMember, command)));
    }

    void SetReadyState()
    {
        int setCommands = 0;
        foreach (PartyMember partyMember in managedBattleState.PlayerPartyPointer.PartyMembers)
        {
            BattleCommand chosenCommand = managedBattleState.BattleCommands.FirstOrDefault(bc => bc.ActingMember == partyMember);

            if (chosenCommand != null)
            {
                partyMember.Hud.SetCommand(chosenCommand, CancelChoice);
                setCommands++;
            }
            else
            {
                partyMember.Hud.SetReady(CommandSelected);
            }
        }

        if (setCommands == 0)
        {
            battleSceneHelperTools.SetCommitButtonState(BattleSceneHelperTools.CommitButtonState.NoneSet);
        }
        else if (setCommands < managedBattleState.PlayerPartyPointer.PartyMembers.Count)
        {
            battleSceneHelperTools.SetCommitButtonState(BattleSceneHelperTools.CommitButtonState.SomeSet);
        }
        else
        {
            battleSceneHelperTools.SetCommitButtonState(BattleSceneHelperTools.CommitButtonState.AllSet);
        }
    }

    void CancelChoice(PartyMember forMember)
    {
        managedBattleState.BattleCommands.RemoveAll(bc => bc.ActingMember == forMember);
        forMember.Hud.SetReady(CommandSelected);
    }

    void GoToResolve()
    {
        foreach (FoeMember foe in managedBattleState.Opponents.OpposingMembers)
        {
            if (foe.CurProblemJuice > 0)
            {
                managedBattleState.BattleCommands.Add(new BattleCommand(foe, managedBattleState.PlayerPartyPointer.GetRandom(), "poke"));
            }
        }
        battleSceneHelperTools.StartCoroutine(stateMachineInstance.ChangeToState(new ResolveState(stateMachineInstance, managedBattleState)));
    }
}
