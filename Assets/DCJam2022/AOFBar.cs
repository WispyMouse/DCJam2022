using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AOFBar : MonoBehaviour
{
    public TMP_Text AOFLabel;
    public Slider AOFSlider;
    public static AOFBar Instance { get; set; }

    private void Start()
    {
        Instance = this;
    }

    public void SetValue(int cur, int max)
    {
        AOFSlider.maxValue = max;
        AOFSlider.value = cur;
        AOFLabel.text = cur.ToString();
    }
}
