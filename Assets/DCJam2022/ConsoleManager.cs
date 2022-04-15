using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using System.Text;

public class ConsoleManager : MonoBehaviour
{
    public TMP_Text LogText;
    public static ConsoleManager Instance { get; private set; }

    FileStream file { get; set; }

    private void Start()
    {
        Instance = this;
        file = new FileStream("combatlog.txt", FileMode.OpenOrCreate);
    }

    public void AddToLog(string toAdd)
    {
        LogText.text += $"\n{toAdd}";

        file.Write(Encoding.UTF8.GetBytes($"\n{toAdd}"));
    }

    public void LogWithoutConsole(string toAdd)
    {
        file.Write(Encoding.UTF8.GetBytes($"\n{toAdd}"));
    }

    public void Clear()
    {
        LogText.text = "";
    }

    private void OnDisable()
    {
        if (file != null)
        {
            file.Close();
        }
        
    }
}
