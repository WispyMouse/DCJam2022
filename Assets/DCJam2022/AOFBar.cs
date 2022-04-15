using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AOFBar : WarrencrawlBar
{
    public TMP_Text AOFLabel;
    public Slider AOFSlider;
    public static AOFBar Instance { get; set; }

    private void Start()
    {
        Instance = this;
    }

    public override void SetValue(int cur, int max)
    {
        base.SetValue(cur, max);
        AOFLabel.text = cur.ToString();
    }
}
