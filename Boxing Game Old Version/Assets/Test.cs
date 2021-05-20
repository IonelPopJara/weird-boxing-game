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
    public float smoothening = 5f;

    public string weaData;
    public float positionX;
    public float positionY;

    private void Start()
    {
        UdpManager.instance.Connect("127.0.0.1", 8078);

        UdpManager.instance.SendUDPMessage("data");
    }

    private void Update()
    {
        weaData = UdpManager.instance.receivedData;
        positionX = UdpManager.instance.positionX;
        positionY = UdpManager.instance.positionY;
    }
}
