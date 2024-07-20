using System;
using UnityEngine;

public class HandleInputBlocker : MonoBehaviour
{
    
    [SerializeField] private BaseInputManager inputManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Handle"))
        {
            inputManager.BlockedByHandle = true;
            Debug.Log("Blocked by Handle");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Handle"))
        {
            inputManager.BlockedByHandle = false;
            Debug.Log("Unblocked by Handle");
        }
    }
}