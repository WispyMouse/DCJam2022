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
        BaseSlider.maxValue = maxValue;
        BaseSlider.value = newValue;

        if (FadingCoroutine != null)
        {
            StopCoroutine(FadingCoroutine);
            FadingCoroutine = null;
        }
        
        if (curValuePointer != newValue)
        {
            if (gameObject.activeInHierarchy)
            {
                FadingCoroutine = StartCoroutine(FadeToValue(newValue));
            }
            else
            {
                Backbar.fillAmount = newValue / maxValue;
            }
        }

        curValuePointer = newValue;
    }

    IEnumerator FadeToValue(int toValue)
    {
        float totalProgress = 0;
        float curSliderValue = curValuePointer;
        float startingValue = curValuePointer;

        yield return new WaitForSeconds(HangTimeAfterLoss);

        while (totalProgress <= TimeForFullBarDrop)
        {
            totalProgress += Time.deltaTime;
            curSliderValue = Mathf.Lerp(startingValue, toValue, totalProgress / TimeForFullBarDrop);
            Backbar.GetComponent<RectTransform>().anchorMax = new Vector2((float)curSliderValue / BaseSlider.maxValue, 1f);
            yield return new WaitForEndOfFrame();
        }

        curValuePointer = toValue;
    }
}
