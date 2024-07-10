using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotationreader : MonoBehaviour
{
    
    [SerializeField] private Transform _target;
    [SerializeField] private UiLogger _uiLogger;
    
    void Update()
    {
        // euler angles
        _uiLogger.Log(_target.rotation.eulerAngles.ToString());
        
        // quaternion
        // _uiLogger.Log(_target.rotation.ToString());
    }
}
