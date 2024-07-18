using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEraser2D : MonoBehaviour
{
    [SerializeField] protected BaseInputManager inputManager;

    private void OnTriggerEnter(Collider other)
    {
        if (inputManager.RightHandIsErasing2D && other.gameObject.CompareTag("Line"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
