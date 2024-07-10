using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Action = Meta.XR.Locomotion.Teleporter.Action;

public class UiLogger : MonoBehaviour
{
    public TextMeshProUGUI outputText;
    public TextMeshProUGUI outputText2;
    
    public BaseInputManager inputManager;
    
    

    public void Start()
    {
        inputManager.ChangeEffect += () => Log("ChangeEffect", 2);
        inputManager.SwitchMode += () => Log("SwitchMode", 2);
        inputManager.MainMenu += () => Log("MainMenu",2);
        inputManager.TurnOnColorPicker += () => Log("TurnOnColorPicker",2);
        inputManager.TurnOffColorPicker += () => Log("TurnOffColorPicker",2);
        inputManager.Undo += () => Log("Undo",2);
        inputManager.Redo += () => Log("Redo",2);
        inputManager.ToggleBrushEraser += () => Log("ToggleBrushEraser",2);
    }

    public void Log(string message, int output = 1)
    {
        if (output == 1)
        {
            outputText.text = message;
        }
        else
        {
            outputText2.text = message;
        }
    }
}
