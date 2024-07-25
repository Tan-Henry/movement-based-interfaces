using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiReset : MonoBehaviour
{
    
    [SerializeField] private BaseInputManager inputManager;
    [SerializeField] private GameObject head;
   
    
    void Start()
    {
        inputManager.ResetMenu += () =>
        {
            transform.position = head.transform.position + head.transform.forward * 10;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
