using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TCPServer : MonoBehaviour
{
    private TcpListener listener;
    private Thread listenerThread;
    
    public event Action ShakeLeft;
    public event Action ShakeRight;
    public event Action ShakeBoth;
    
    private float shakeLeftTime = -1f;
    private float shakeRightTime = -1f;
    private float simultaneousTimeWindow = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        listenerThread = new Thread(new ThreadStart(ServerStart));
        listenerThread.IsBackground = true;
        listenerThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ServerStart()
    {
        listener = new TcpListener(IPAddress.Any, 5005);
        listener.Start();
        
        Byte[] bytes = new Byte[1024];

        while (true)
        {
            using (TcpClient client = listener.AcceptTcpClient())
            {
                NetworkStream stream = client.GetStream();
                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    string data = Encoding.ASCII.GetString(bytes, 0, i);
                    // check if data contains the word "shake"
                    if (data.Contains("shakeleft"))
                    {
                        HandleShakeLeft();
                    }
                    else if (data.Contains("shakeright"))
                    {
                        HandleShakeRight();
                    }
                    
                }
            }
        }
    }
    
    void HandleShakeLeft()
    {
        shakeLeftTime = Time.time;
        StartCoroutine(CheckSimultaneousShakes());
    }

    void HandleShakeRight()
    {
        shakeRightTime = Time.time;
        StartCoroutine(CheckSimultaneousShakes());
    }

    IEnumerator CheckSimultaneousShakes()
    {
        yield return new WaitForSeconds(simultaneousTimeWindow);

        if (Math.Abs(shakeLeftTime - shakeRightTime) <= simultaneousTimeWindow)
        {
            ShakeBoth?.Invoke();
            shakeLeftTime = -1f;
            shakeRightTime = -1f;
        }
        else
        {
            if (shakeLeftTime > 0)
            {
                ShakeLeft?.Invoke();
                shakeLeftTime = -1f;
            }

            if (shakeRightTime > 0)
            {
                ShakeRight?.Invoke();
                shakeRightTime = -1f;
            }
        }
    }
    
    void OnApplicationQuit()
    {
        listener.Stop();
        listenerThread.Abort();
    }
}
