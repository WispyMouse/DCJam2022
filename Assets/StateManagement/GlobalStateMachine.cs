using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the <see cref="IGameplayState"/>s of the game.
/// If you want to transition from one state to another, that goes through here.
/// Receives input and passes it to the current active state.
/// </summary>
public class GlobalStateMachine
{
    /// <summary>
    /// Cached reference to the last set of controls used.
    /// </summary>
    WarrencrawlInputs lastActiveControls { get; set; }

    /// <summary>
    /// The stack of states currently loaded in to memory.
    /// States underneath the top may be "inactive" and out of memory, and the top state may not be fully "active".
    /// </summary>
    Stack<IGameplayState> PresentStates { get; set; } = new Stack<IGameplayState>();

    /// <summary>
    /// Constructor for the GlobalStateMachine.
    /// </summary>
    /// <param name="inputs">The input map used.</param>
    public GlobalStateMachine(WarrencrawlInputs inputs)
    {
        this.lastActiveControls = inputs;
    }

    /// <summary>
    /// Returns the current top level state, if there is one.
    /// </summary>
    public IGameplayState CurrentState
    {
        get
        {
            if (PresentStates.TryPeek(out IGameplayState currentState))
            {
                return currentState;
            }

            return null;
        }
    }
    
    /// <summary>
    /// Transitions in to a new state, finishing when complete.
    /// The old state is fully unloaded and removed.
    /// </summary>
    /// <param name="newState">The state to transition in to.</param>
    /// <returns>Yieldable IEnumerator.</returns>
    public IEnumerator ChangeToState(IGameplayState newState)
    {
        IGameplayState oldState = CurrentState;
        if (oldState != null)
        {
            yield return oldState.AnimateTransitionOut(newState);
            yield return oldState.ExitState(newState);
            PresentStates.Pop();
        }

        PresentStates.Push(newState);
        yield return WarmUpAndStartCurrentState(oldState, true);
    }

    /// <summary>
    /// Transitions to a new state, finishing when complete.
    /// The old state remains, lower on the <see cref="PresentStates"/> stack.
    /// </summary>
    /// <param name="newState">The state to transition in to.</param>
    /// <returns>Yieldable IEnumerator.</returns>
    public IEnumerator PushNewState(IGameplayState newState)
    {
        IGameplayState oldState = CurrentState;

        oldState?.UnsetControls(lastActiveControls);

        yield return oldState?.AnimateTransitionOut(newState);
        yield return oldState?.ChangeUp(newState);
        PresentStates.Push(newState);
        yield return WarmUpAndStartCurrentState(oldState, true);
    }

    /// <summary>
    /// Transitions out of the current state to the one below it.
    /// </summary>
    /// <returns>Yieldable IEnumerator.</returns>
    public IEnumerator EndCurrentState(bool skipWarmup = false)
    {
        IGameplayState oldState = CurrentState;
        PresentStates.Pop();

        oldState?.UnsetControls(lastActiveControls);

        PresentStates.TryPeek(out IGameplayState nextState);
        yield return oldState?.AnimateTransitionOut(nextState);
        yield return oldState?.ExitState(nextState);

        if (!skipWarmup)
        {
            yield return WarmUpAndStartCurrentState(oldState, false);
        }
        
    }

    /// <summary>
    /// Prepares and starts the <see cref="CurrentState"/>, if there is one.
    /// </summary>
    /// <param name="lastState">The previously active state.</param>
    private IEnumerator WarmUpAndStartCurrentState(IGameplayState lastState, bool shouldInitialize)
    {
        if (CurrentState == null)
        {
            yield break;
        }

        yield return CurrentState.Load();

        if (shouldInitialize)
        {
            yield return CurrentState.Initialize();
        }

        yield return CurrentState.AnimateTransitionIn(lastState);
        CurrentState.SetControls(lastActiveControls);
        yield return CurrentState.StartState(this, lastState);
    }

    /// <summary>
    /// Ends every state currently in the machine.
    /// </summary>
    public IEnumerator CollapseAllStates()
    {
        while (CurrentState != null)
        {
            yield return CurrentState.ExitState(null);
            PresentStates.Pop();
        }
    }
}
