using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using Unity.VisualScripting;

public class SerialController : MonoBehaviour
{
    private SerialPort sp = new SerialPort("COM8", 9600);
    public GameObject finger1;
    public GameObject finger2;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!sp.IsOpen)
        {
            sp.Open();
        }
    }

    // Update is called once per frame
    void Update()
    {
        string message = sp.ReadLine();
        string[] values = message.Split(' ');

        float sensorValue1 = float.Parse(values[0]);
        float sensorValue2 = float.Parse(values[1]);
        
        float normalizedSensor1 = Mathf.InverseLerp(0, 15000, sensorValue1);
        float normalizedSensor2 = Mathf.InverseLerp(0, 15000, sensorValue2);

        finger1.transform.localScale = new Vector3(1, normalizedSensor1, 1);
        finger2.transform.localScale = new Vector3(1, normalizedSensor2, 1);
    }

    private void OnApplicationQuit()
    {
        if (sp.IsOpen)
        {
            sp.Close();
        }
    }
}
