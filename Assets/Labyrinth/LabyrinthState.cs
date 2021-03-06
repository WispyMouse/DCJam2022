using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LabyrinthState : SceneLoadingGameplayState
{
    public override string SceneName => "Labyrinth";

    /// <summary>
    /// The GameLevel to initialize and load in.
    /// If this is null, we try to pull a default level from <see cref="LabyrinthSceneHelperTools"/>.
    /// </summary>
    public GameLevel LevelToLoad { get; private set; }

    /// <summary>
    /// The current game object representing the player's point of view.
    /// </summary>
    public PointOfView PointOfViewInstance { get; private set; }

    /// <summary>
    /// A mechanism for providing a level.
    /// </summary>
    private IGameLevelProvider GameLevelProvider { get; set; }

    /// <summary>
    /// Reports when an exclusive animation has finished.
    /// </summary>
    public event EventHandler LockingAnimationFinished;

    /// <summary>
    /// Pointer to the active LabyrinthSceneHelperTools. Retrieved on Load.
    /// </summary>
    public LabyrinthSceneHelperTools HelperTools { get; private set; }

    /// <summary>
    /// Pointer to the input handler for this state. Retrieved on load from LabyrinthSceneHelperTools.
    /// </summary>
    public LabyrinthInputHandler InputHandler { get; private set; }

    /// <summary>
    /// Pointer to an Animation Handler for this state.
    /// </summary>
    public LabyrinthAnimationHandler AnimationHandler { get; private set; }

    public CombatClock ActiveCombatClock { get; private set; }

    /// <summary>
    /// Constructor for LabyrinthState.
    /// </summary>
    /// <param name="levelProvider">A mechanism for providing a level.</param>
    public LabyrinthState(IGameLevelProvider levelProvider)
    {
        this.GameLevelProvider = levelProvider;
    }

    public override IEnumerator Load()
    {
        yield return base.Load();

        HelperTools = GameObject.FindObjectOfType<LabyrinthSceneHelperTools>();

        if (HelperTools == null)
        {
            Debug.LogWarning($"No {nameof(LabyrinthSceneHelperTools)} found in the scene.");
            yield break;
        }

        InputHandler = HelperTools.InputHandler;
        AnimationHandler = HelperTools.AnimationHandler;

        LevelToLoad = GameLevelProvider.GetLevel();

        if (LevelToLoad == null)
        {
            Debug.LogWarning($"No {nameof(LevelToLoad)} detected. Either pass a {nameof(GameLevel)} to the {nameof(LabyrinthLevel)} constructor, or have a {nameof(LabyrinthSceneHelperTools.DefaultLevel)} set in {nameof(LabyrinthSceneHelperTools)}.");
            yield break;
        }

        if (!string.IsNullOrEmpty(LevelToLoad.Scene) && !StaticSceneTools.IsSceneLoaded(LevelToLoad.Scene))
        {
            yield return StaticSceneTools.LoadAddressableSceneAdditvely(LevelToLoad);
        }

        if (PointOfViewInstance == null)
        {
            PointOfViewInstance = GameObject.FindObjectOfType<PointOfView>();
            PointOfViewInstance.CurFacing = LevelToLoad.LabyrinthData.StartingFacing;
            PointOfViewInstance.CurCoordinates = LevelToLoad.LabyrinthData.StartingCoordinate;
        }

        ActiveCombatClock = new CombatClock();

        HelperTools.InteractiveButtonPressed.RemoveAllListeners();
        HelperTools.InteractiveButtonPressed.AddListener(() => { SceneHelperInstance.StartCoroutine(Interact()); });

        ScanInteractivesUsingFlags();
    }

    public override IEnumerator StartState(GlobalStateMachine globalStateMachine, IGameplayState previousState)
    {
        LockingAnimationFinished?.Invoke(this, new EventArgs());

        ScanInteractivesUsingFlags();

        yield return base.StartState(globalStateMachine, previousState);

        switch (LevelToLoad.CombatClockEnabled)
        {
            case true:
                ActiveCombatClock.ResetCombatClock();
                ActiveCombatClock.Enable();
                break;
            case false:
                ActiveCombatClock.Disable();
                break;
        }

        LabyrinthCell cellAtStart = LevelToLoad.LabyrinthData.CellAtCoordinate(PointOfViewInstance.CurCoordinates);

        if (cellAtStart == null)
        {
            Debug.LogError($"The starting tile at {PointOfViewInstance.CurCoordinates} doesn't exist in the level's map.");
        }

        PointOfViewInstance.transform.rotation = Quaternion.Euler(0, PointOfViewInstance.CurFacing.Degrees(), 0);
        PointOfViewInstance.transform.position = cellAtStart.Worldspace;

        CheckAndPresentAccessibleInteractive();

        ScanInteractivesUsingFlags();
    }

    public override IEnumerator ChangeUp(IGameplayState nextState)
    {
        yield return base.ChangeUp(nextState);
        HelperTools.InteractiveButtonPressed.RemoveAllListeners();
        HelperTools.InteractiveInFrontPanel.SetActive(false);
    }

    public override IEnumerator ExitState(IGameplayState nextState)
    {
        if (!string.IsNullOrEmpty(LevelToLoad.Scene) && StaticSceneTools.IsSceneLoaded(LevelToLoad.Scene))
        {
            yield return StaticSceneTools.UnloadScene(LevelToLoad.Scene);
        }

        HelperTools.InteractiveButtonPressed.RemoveAllListeners();
        HelperTools.InteractiveInFrontPanel.SetActive(false);

        yield return base.ExitState(nextState);
    }

    public override void SetControls(WarrencrawlInputs controls)
    {
        controls.Labyrinth.Enable();
        controls.Labyrinth.SetCallbacks(InputHandler);
    }

    /// <summary>
    /// Attempts to move in the direction provided.
    /// This also processes any events that should happen as a result of moving.
    /// </summary>
    /// <param name="offset">Direction to move.</param>
    public IEnumerator Step(Vector3Int offset)
    {
        UnsetAccessibleInteractive();

        CellCoordinates newCoordinates = PointOfViewInstance.CurCoordinates + offset;

        LabyrinthCell cellAtPosition = LevelToLoad.LabyrinthData.CellAtCoordinate(newCoordinates);
        CheckAndPresentAccessibleInteractive();

        if (cellAtPosition == null)
        {
            Debug.Log("Couldn't move there, the cell doesn't exist.");
            LockingAnimationFinished?.Invoke(this, new EventArgs());
            yield break;
        }

        if (!cellAtPosition.DefaultWalkable)
        {
            Debug.Log("Couldn't move there, the cell is not walkable.");
            LockingAnimationFinished?.Invoke(this, new EventArgs());
            yield break;
        }

        foreach (InteractiveData interactive in LevelToLoad.LabyrinthData.InteractivesAtCoordinate(newCoordinates))
        {
            if (interactive.ShouldEnableBasedOnSetFlags(SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData))
            {
                if (interactive.Kind == InteractiveKind.SameTileInteractive)
                {
                    continue;
                }
                else
                {
                    Debug.Log("Couldn't move there, an interactive is in the spot and enabled and doesn't share space.");
                    LockingAnimationFinished?.Invoke(this, new EventArgs());
                    yield break;
                }
            }
        }

        yield return AnimationHandler.StepTo(PointOfViewInstance, cellAtPosition);

        PointOfViewInstance.CurCoordinates = newCoordinates;

        ActiveCombatClock.StepTaken();

        if (ActiveCombatClock.ShouldEncounterStart())
        {
            yield return StateMachineInstance.PushNewState(new BattleState());
        }

        CheckAndPresentAccessibleInteractive();

        LockingAnimationFinished?.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Rotates the point of view towards the direction provided.
    /// </summary>
    /// <param name="newFacing">The new direction to face.</param>
    public IEnumerator Rotate(Direction newFacing)
    {
        UnsetAccessibleInteractive();
        yield return AnimationHandler.Rotate(PointOfViewInstance, newFacing);

        PointOfViewInstance.CurFacing = newFacing;
        CheckAndPresentAccessibleInteractive();
        LockingAnimationFinished?.Invoke(this, new EventArgs());
        yield break;
    }

    /// <summary>
    /// Attempt to interact with the object in front of the player's view.
    /// </summary>
    public IEnumerator Interact()
    {
        InteractiveData accessibleInteractive = AccessibleInteractive();

        if (accessibleInteractive == null)
        {
            LockingAnimationFinished?.Invoke(this, new EventArgs());
            yield break;
        }

        // todo: other kinds of interactives!
        switch (accessibleInteractive.Kind)
        {
            case InteractiveKind.Stairs:
                yield return StateMachineInstance.ChangeToState(new TownState());
                break;
            case InteractiveKind.OutsideTileInteractive:
                yield return StateMachineInstance.PushNewState(new HandleObstacleState(this, SceneHelperInstance, accessibleInteractive.ObstacleEventData));
                break;
        }

        LockingAnimationFinished?.Invoke(this, new EventArgs());
    }

    public InteractiveData AccessibleInteractive()
    {
        LabyrinthCell curCell = LevelToLoad.LabyrinthData.CellAtCoordinate(PointOfViewInstance.CurCoordinates);

        if (curCell == null)
        {
            return null;
        }

        IEnumerable<InteractiveData> interactivesAtCoordinate = LevelToLoad.LabyrinthData.InteractivesAtCoordinate(curCell.Coordinate).Where(x => x.Kind == InteractiveKind.SameTileInteractive);

        if (curCell != null && interactivesAtCoordinate.Any())
        {
            InteractiveData sameTileInteractive = interactivesAtCoordinate.FirstOrDefault(x => x.ShouldEnableBasedOnSetFlags(SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData));

            if (sameTileInteractive != null)
            {
                return sameTileInteractive;
            }
        }

        CellCoordinates coordinatesInFacing = PointOfViewInstance.CurCoordinates + PointOfViewInstance.CurFacing.Forward();
        LabyrinthCell forwardCell = LevelToLoad.LabyrinthData.CellAtCoordinate(coordinatesInFacing);

        if (forwardCell == null)
        {
            return null;
        }

        IEnumerable<InteractiveData> interactivesAtCell = LevelToLoad.LabyrinthData.InteractivesAtCoordinate(forwardCell.Coordinate);

        return interactivesAtCell.FirstOrDefault(x => x.ShouldEnableBasedOnSetFlags(SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData));
    }

    public override void UnsetControls(WarrencrawlInputs inputs)
    {
        inputs.Labyrinth.SetCallbacks(null);
        inputs.Labyrinth.Disable();
    }

    void ScanInteractivesUsingFlags()
    {
        foreach (InteractiveData interactive in LevelToLoad.LabyrinthData.LabyrinthInteractives)
        {
            if (interactive != null)
            {
                if (interactive.ShouldEnableBasedOnSetFlags(SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData))
                {
                    interactive.WorldInteractive?.EnableInteractive();
                    interactive.IsActive = true;
                }
                else
                {
                    interactive.WorldInteractive?.DisableInteractive();
                    interactive.IsActive = false;
                }
            }
        }
    }

    public override void SetSceneActiveState(bool toAcctive)
    {
        base.SetSceneActiveState(toAcctive);

        if (LevelToLoad == null || string.IsNullOrEmpty(LevelToLoad.Scene))
        {
            return;
        }

        if (StaticSceneTools.IsSceneLoaded(LevelToLoad.Scene))
        {
            foreach (GameObject rootObj in SceneManager.GetSceneByName(LevelToLoad.Scene).GetRootGameObjects())
            {
                rootObj.SetActive(toAcctive);
            }
        }
    }

    public void UnsetAccessibleInteractive()
    {
        HelperTools.InteractiveButtonPressed.RemoveAllListeners();
        HelperTools.InteractiveInFrontPanel.SetActive(false);
    }

    public void CheckAndPresentAccessibleInteractive()
    {
        InteractiveData accessibleInteractive = AccessibleInteractive();
        if (accessibleInteractive != null)
        {
            HelperTools.InteractiveButtonPressed.RemoveAllListeners();
            HelperTools.InteractiveButtonPressed.AddListener(() => { SceneHelperInstance.StartCoroutine(Interact()); });
            HelperTools.InteractiveInFrontPanel.SetActive(true);
        }
        else
        {
            UnsetAccessibleInteractive();
        }
    }
}
