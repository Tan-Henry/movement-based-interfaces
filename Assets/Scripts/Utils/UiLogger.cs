using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiLogger : MonoBehaviour
{
    public TextMeshProUGUI outputText;
    
    public void Log(string message)
    {
        outputText.text = message;
    }
}
