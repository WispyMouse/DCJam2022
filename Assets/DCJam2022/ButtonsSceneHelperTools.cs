using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSceneHelperTools : SceneHelperTools
{
    public List<EncounterBattle> Encounters = new List<EncounterBattle>();
    public Button ButtonPF;
    public Transform ButtonParent;

    protected override IEnumerator StartChild()
    {
        yield return base.StartChild();

        for (int ii = 0; ii < Encounters.Count; ii++)
        {
            EncounterBattle encounter = Encounters[ii];
            Button newButton = Instantiate(ButtonPF, ButtonParent);
            newButton.GetComponentInChildren<TMP_Text>().text = encounter.EncounterName;

            int indexCapture = ii;
            newButton.onClick.AddListener(() => StartEncounter(indexCapture));
        }
    }

    public override IGameplayState GetNewDemoState()
    {
        return new ButtonsState();
    }

    public void StartEncounter(int encounterId)
    {
        Debug.Log($"Begin encounterId {encounterId}");
        SceneHelperInstance.StartCoroutine(SceneHelper.GlobalStateMachineInstance.PushNewState(new BattleState(Encounters[encounterId])));
    }
}
