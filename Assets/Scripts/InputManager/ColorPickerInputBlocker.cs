using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerInputBlocker : MonoBehaviour
{
    
    [SerializeField] private BaseInputManager inputManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ColorPicker"))
        {
            inputManager.BlockedByColorPicker = true;
            Debug.Log("Blocked by ColorPicker");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ColorPicker"))
        {
            inputManager.BlockedByColorPicker = false;
            Debug.Log("Unblocked by Handle");
        }
    }
}
