using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class SerialServer : BaseSensorServer
{
    public override event Action ShakeLeft;
    public override event Action ShakeRight;
    public override event Action ShakeBoth;

    private SerialPort serialPortLeft;
    private SerialPort serialPortRight;
    private Thread readThread;
    private bool isRunning;
    private bool shakeLeftDetected;
    private bool shakeRightDetected;

    void Start()
    {
        serialPortLeft = new SerialPort("COM8", 9600);
        serialPortRight = new SerialPort("COM11", 9600);

        // Öffne die seriellen Verbindungen
        serialPortLeft.Open();
        serialPortRight.Open();
        
        isRunning = true;
        shakeLeftDetected = false;
        shakeRightDetected = false;

        // Start the thread that reads data
        readThread = new Thread(ReadSerialData);
        readThread.Start();
        Debug.Log("SerialServer started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void ReadSerialData()
    {
        while (isRunning)
        {
            try
            {
                if (serialPortLeft.IsOpen && serialPortLeft.BytesToRead > 0)
                {
                    string dataLeft = serialPortLeft.ReadLine().Trim();
                    if (dataLeft.Contains("shake"))
                    {
                        shakeLeftDetected = true;
                    }
                }

                if (serialPortRight.IsOpen && serialPortRight.BytesToRead > 0)
                {
                    string dataRight = serialPortRight.ReadLine().Trim();
                    if (dataRight.Contains("shake"))
                    {
                        shakeRightDetected = true;
                    }
                }
                
                if (shakeLeftDetected && shakeRightDetected)
                {
                    ShakeBoth?.Invoke();
                    shakeLeftDetected = false;
                    shakeRightDetected = false;
                }
                else
                {
                    if (shakeLeftDetected)
                    {
                        ShakeLeft?.Invoke();
                        shakeLeftDetected = false;
                    }

                    if (shakeRightDetected)
                    {
                        ShakeRight?.Invoke();
                        shakeRightDetected = false;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Serieller Fehler: " + e.Message);
            }
            
            Thread.Sleep(100);
        }
    }
    
    void OnApplicationQuit()
    {
        // Stoppe den Thread
        isRunning = false;
        readThread.Join();

        // Schließe die seriellen Verbindungen
        if (serialPortLeft != null && serialPortLeft.IsOpen)
        {
            serialPortLeft.Close();
        }

        if (serialPortRight != null && serialPortRight.IsOpen)
        {
            serialPortRight.Close();
        }
    }
}
