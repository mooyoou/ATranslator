using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utility;

public class DebugConsole : MonoBehaviour
{
    public TextConsoleSimulator console;
    public ScrollRect consolescrollRect;
    public TMP_InputField consoleInput;

    public void HandleInputEnd(string inputText)
    {
        
        console.AddText($"> {inputText}");
        HandleSpecialCommand(inputText);
        
        
        
        Canvas.ForceUpdateCanvases();      
        consolescrollRect.verticalNormalizedPosition = 0f;
        consoleInput.text = "";
    }

    /// <summary>
    /// 处理特殊语句
    /// </summary>
    /// <param name="cmd"></param>
    private void HandleSpecialCommand(string cmd)
    {
        switch (cmd.ToLower())
        {
            case "cls":
                console.ClearScreen();
                break;
            case "version":
                console.AddText($" {Application.productName} : {Application.version} ");
                break;
            default:
                break;
        }
    } 
    
}
