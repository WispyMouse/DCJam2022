using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayIndicator : MonoBehaviour
{
    public TMP_Text DayValue;
    public TownSceneHelperTools SceneHelperTools;

    private void OnEnable()
    {
        StartCoroutine(UpdateLabel());
    }

    public IEnumerator UpdateLabel()
    {
        while (SceneHelperTools?.SceneHelperInstance?.SaveDataManagerInstance == null)
        {
            yield return new WaitForEndOfFrame();
        }

        DayValue.text = SceneHelperTools.SceneHelperInstance.SaveDataManagerInstance.CurrentSaveData.Day.ToString();
    }
}
