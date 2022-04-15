using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarrencrawlBar : MonoBehaviour
{
    const float TimeForFullBarDrop = .6f;
    const float HangTimeAfterLoss = .3f;

    int curValuePointer { get; set; } = -1;
    Coroutine FadingCoroutine { get; set; }

    public Slider BaseSlider;
    public Image Backbar;

    public virtual void SetValue(int newValue, int maxValue)
    {
        if (curValuePointer <= 0)
        {
            curValuePointer = newValue;
        }

        BaseSlider.maxValue = maxValue;

        if (maxValue >= 0)
        {
            Backbar.fillAmount = curValuePointer / maxValue;
        }
        else
        {
            Backbar.fillAmount = 1f;
        }

        if (FadingCoroutine != null)
        {
            StopCoroutine(FadingCoroutine);
            FadingCoroutine = null;
        }
        
        if (gameObject.activeInHierarchy)
        {
            FadingCoroutine = StartCoroutine(FadeToValue(newValue));
        }
        else
        {
            Backbar.fillAmount = newValue / maxValue;
        }
    }

    IEnumerator FadeToValue(int toValue)
    {
        float totalProgress = 0;
        int curSliderValue = curValuePointer;

        while (totalProgress <= TimeForFullBarDrop)
        {
            totalProgress += Time.deltaTime;
            curSliderValue = (int)Mathf.Lerp(curValuePointer, toValue, totalProgress / TimeForFullBarDrop);
            BaseSlider.value = curSliderValue;
            Backbar.GetComponent<RectTransform>().anchorMax = new Vector2(curSliderValue / BaseSlider.maxValue, 1f);
            yield return new WaitForEndOfFrame();
        }

        curValuePointer = curSliderValue;
    }
}
