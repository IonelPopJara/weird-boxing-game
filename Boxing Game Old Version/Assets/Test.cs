using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Tool;

public class Test : MonoBehaviour
{
    public UdpManager udpManager;
    public string weaData;
    public double positionX;
    public double positionY;

    private void Start()
    {
        udpManager = new UdpManager();
        udpManager.Connect("127.0.0.1", 8078);

        udpManager.SendMessage("data");
    }

    private void Update()
    {
        weaData = udpManager.receivedData;
        positionX = udpManager.positionX;
        positionY = udpManager.positionY;
    }

    private void OnApplicationQuit()
    {
        udpManager.StopThreads();
    }

    private void OnDestroy()
    {
        udpManager.StopThreads();
    }
}
