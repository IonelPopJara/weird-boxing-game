using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpReceiver : MonoBehaviour
{
    public int recv;
    public byte[] data = new byte[1024];

    public IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 904);

    Socket newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    IPEndPoint sender;
    EndPoint tmpRemote;

    private void Start()
    {
        newSocket.Bind(endPoint);

        Debug.Log("Waiting for client...");

        sender = new IPEndPoint(IPAddress.Any, 904);
        tmpRemote = (EndPoint)sender;

        recv = newSocket.ReceiveFrom(data, ref tmpRemote);

        Debug.Log($"Message received from {tmpRemote.ToString()}");
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
    }

    private void Update()
    {
        if(!newSocket.Connected)
        {
            Debug.Log("Client Disconnected...");
            return;
        }

        data = new byte[1024];
        recv = newSocket.ReceiveFrom(data, ref tmpRemote);

        if (recv == 0)
            return;

        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
    }
}
