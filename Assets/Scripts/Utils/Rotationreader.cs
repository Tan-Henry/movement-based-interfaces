using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotationreader : MonoBehaviour
{
    
    [SerializeField] private Transform _target;
    [SerializeField] private UiLogger _uiLogger;
    [SerializeField] private BaseInputManager _inputManager;
    void Update()
    {
        // euler angles
        _uiLogger.Log(_target.rotation.eulerAngles.ToString());
        
        _inputManager.SwitchMode += () => _uiLogger.Log("SwitchMode");
        
        _inputManager.ChangeEffect += DoSomething;

        if (_inputManager.RightHandIsDrawing)
        {
            DoSomething();
        }
        
        // quaternion
        // _uiLogger.Log(_target.rotation.ToString());
    }
    
    private void DoSomething()
    {
        _uiLogger.Log("doSomething");
    }
}
