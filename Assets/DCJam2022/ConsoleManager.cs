using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    public TMP_Text LogText;
    public static ConsoleManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    public void AddToLog(string toAdd)
    {
        LogText.text += $"\n{toAdd}";
    }

    public void Clear()
    {
        LogText.text = "";
    }
}
