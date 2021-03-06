using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// The global entry point for regular gameplay of this engine.
/// This object exists in a scene and bootstraps the code and gets things going.
/// If a scene with just this is loaded, it'll get the game started on the MainMenu.
/// If another scene is open when this is loaded, this'll try to load the game to an appropriate state.
/// </summary>
public class SceneHelper : MonoBehaviour
{
    public static GlobalStateMachine GlobalStateMachineInstance { get; private set; }
    public static WarrencrawlInputs Inputs { get; private set; }

    public Transitions TransitionsInstance;
    public PlayerParty PlayerParty { get; set; }
    public SaveDataManager SaveDataManagerInstance;
    public List<DelverProfile> DefaultDelvers;


    private void Awake()
    {
        if (Inputs == null)
        {
            Inputs = new WarrencrawlInputs();
        }

        if (PlayerParty == null)
        {
            Debug.Log("Setting PlayerParty");
            PlayerParty = new PlayerParty() { MaxAOF = 10, CurAOF = 10 };
            foreach (DelverProfile profile in DefaultDelvers)
            {
                PlayerParty.AddPartyMember(profile);
            }
        }

        if (GlobalStateMachineInstance == null)
        {
            GlobalStateMachineInstance = new GlobalStateMachine(Inputs);

            SceneHelperTools bootstrapperTools = GameObject.FindObjectOfType<SceneHelperTools>();

            if (bootstrapperTools == null)
            {
                MainMenuState mainMenuState = new MainMenuState();
                StartCoroutine(GlobalStateMachineInstance.ChangeToState(mainMenuState));
            }
            else
            {
                IGameplayState firstState = bootstrapperTools.GetNewDemoState();
                StartCoroutine(GlobalStateMachineInstance.ChangeToState(firstState));
            }
        }
    }

    /// <summary>
    /// Sets the static-ally available variables for the SceneHelper.
    /// When it's seeded this way, it won't create its own <see cref="GlobalStateMachine"/> or <see cref="WarrencrawlInputs"/>.
    /// This is useful for tests, which want to provide their own state machines for testing.
    /// </summary>
    /// <param name="withGSM">Active Global State Machine to use.</param>
    /// <param name="inputs">Active input mappings to use.</param>
    public static IEnumerator SetSceneHelper(GlobalStateMachine withGSM, WarrencrawlInputs inputs)
    {
        GlobalStateMachineInstance = withGSM;
        Inputs = inputs;

        if (!StaticSceneTools.IsSceneLoaded(nameof(SceneHelper)))
        {
            yield return StaticSceneTools.LoadSceneAdditvely(nameof(SceneHelper));
        }
    }
}
