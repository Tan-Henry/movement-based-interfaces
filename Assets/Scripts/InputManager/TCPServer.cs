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
                        ShakeLeft?.Invoke();
                    }
                    else if (data.Contains("shakeright"))
                    {
                        ShakeRight?.Invoke();
                    }
                    
                }
            }
        }
    }
    
    void OnApplicationQuit()
    {
        listener.Stop();
        listenerThread.Abort();
    }
}
