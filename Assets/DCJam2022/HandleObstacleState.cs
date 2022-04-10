using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleObstacleState : IGameplayState
{
    ObstacleEvent EventExperienced { get; set; }
    int curIdPointer { get; set; } = 0;

    public HandleObstacleState(ObstacleEvent forEvent)
    {
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
        ObstacleEventComponent component = EventExperienced.EventComponents.FirstOrDefault(ec => ec.EventId == curIdPointer);

        if (component == null)
        {
            yield return stateMachine.EndCurrentState();
            yield break;
        }

        yield return stateMachine.PushNewState(component.GetNewState(SetPointer));
    }

    public void UnsetControls(WarrencrawlInputs activeInput)
    {
        
    }

    void SetPointer(int toPointer)
    {
        curIdPointer = toPointer;
    }
}
