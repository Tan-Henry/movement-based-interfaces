using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEraser2D : MonoBehaviour
{
    [SerializeField] private BaseInputManager inputManager;
    [SerializeField] private UndoRedoScript _undoRedoScript;

    private void OnTriggerEnter(Collider other)
    {
        if (inputManager.RightHandIsErasing2D && other.gameObject.CompareTag("Line"))
        {
            _undoRedoScript.AddLastLineGameObject(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
    
}
