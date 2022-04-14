using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleObstacleState : IGameplayState
{
    ObstacleEvent EventExperienced { get; set; }
    int curIdPointer { get; set; } = 0;
    SceneHelper sceneHelper { get; set; }

    Action lastExperiencedDelayedAction { get; set; } = null;

    public HandleObstacleState(SceneHelper sceneHelperInstance, ObstacleEvent forEvent)
    {
        sceneHelper = sceneHelperInstance;
        EventExperienced = forEvent;
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
        yield break;
    }

    public IEnumerator ExitState(IGameplayState nextState)
    {
        yield break;
    }

    public IEnumerator Initialize()
    {
        yield break;
    }

    public IEnumerator Load()
    {
        yield break;
    }

    public void SetControls(WarrencrawlInputs activeInput)
    {

    }

    public IEnumerator StartState(GlobalStateMachine stateMachine, IGameplayState previousState)
    {
        // Look up the event matching our current event id; if it is null, then end the state
        // Then ask it for the next state, which can run SetPointer to change the id targeted
        // if the state it returns is null, then repeat the process; there was no new visual changes needed, retake the pointer and move forward

        IGameplayState nextState;
        ObstacleEventComponent component;

        do
        {
            lastExperiencedDelayedAction?.Invoke();
            component = EventExperienced.EventComponents.FirstOrDefault(ec => ec.EventId == curIdPointer);

            if (component == null)
            {
                yield return stateMachine.EndCurrentState();
                yield break;
            }

            lastExperiencedDelayedAction = () => { curIdPointer = component.AfterStateSetPointer(sceneHelper.SaveDataManagerInstance.CurrentSaveData); };
            nextState = component.GetNewState(sceneHelper.SaveDataManagerInstance.CurrentSaveData);

        } while (nextState == null);

        if (component.CloseCurrentState)
        {
            yield return stateMachine.EndCurrentState();

            if (nextState != null)
            {
                yield return stateMachine.ChangeToState(nextState);
            }
        }
        else
        {
            if (nextState != null)
            {
                yield return stateMachine.PushNewState(component.GetNewState(sceneHelper.SaveDataManagerInstance.CurrentSaveData));
            }
        }
    }

    public void UnsetControls(WarrencrawlInputs activeInput)
    {

    }

    void SetPointer(int toPointer)
    {
        curIdPointer = toPointer;
    }
}
