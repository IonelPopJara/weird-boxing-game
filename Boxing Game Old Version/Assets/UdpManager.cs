using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Text;

namespace Tool
{
    public class UdpManager : ISocket
    {
        public delegate void OnGetReceive(string message);
        public OnGetReceive onGetReceive;

        byte[] m_result = new byte[1024];

        Thread m_connectThread;
        UdpClient m_udpClient;
        IPEndPoint m_serverIPEndPoint;
        IPEndPoint m_receiveIPEndPoint;
        Thread m_parseDataThread;
        public string receivedData;
        public double positionX;
        public double positionY;

        bool m_isConnected;

        public bool IsConnected()
        {
            return m_isConnected;
        }

        public void StopThreads()
        {
            m_parseDataThread.Interrupt();
            m_parseDataThread.Abort();
        }

        public void Connect(string ip, int port)
        {
            m_serverIPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            m_udpClient = new UdpClient(0); //!!!!! Mainly must set the port to 0, otherwise you can not receive the server's message
            m_isConnected = true;

            m_receiveIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            m_connectThread = new Thread(new ThreadStart(ReceiveMessage));
            m_connectThread.Start();

            m_parseDataThread = new Thread(new ThreadStart(ParseData));
            m_parseDataThread.Start();
        }

        public void SendMessage(string data)
        {
            if(IsConnected())
            {
                NetBufferWriter writer = new NetBufferWriter();
                Debug.Log("Send Message " + data);
                writer.WriteString(data);
                m_udpClient.Send(writer.Finish(), writer.finishLength, m_serverIPEndPoint);
            }
        }

        public void Disconnect()
        {
            if(IsConnected())
            {
                m_isConnected = false;
            }
            if(m_connectThread != null)
            {
                m_connectThread.Interrupt();
                m_connectThread.Abort();
            }
            if(m_udpClient != null)
            {
                m_udpClient.Close();
            }
        }

        public void ReceiveMessage()
        {
            while(IsConnected())
            {
                try
                {
                    m_result = new byte[1024];
                    m_result = m_udpClient.Receive(ref m_receiveIPEndPoint);
                    NetBufferReader reader = new NetBufferReader(m_result);
                    string msg = reader.ReadString();
                    //Debug.Log("Receive Message " + msg);
                    receivedData = msg;
                } catch(Exception ex)
                {
                    Debug.Log("Receive error " + ex.Message);
                    Disconnect();
                }
            }
        }

        public void ParseData()
        {
            Debug.Log("wea 1");

            while(IsConnected())
            {
                if (receivedData != null)
                {
                    var splitData = receivedData.Split(';');
                    Debug.Log($"wea 2: {splitData}");
                    positionX = Convert.ToDouble(splitData[0]);
                    positionY = Convert.ToDouble(splitData[1]);
                }
            }
        }
    }

    public interface ISocket
    {
        bool IsConnected();

        void Connect(string ip, int port);

        void SendMessage(string data);

        void Disconnect();

        void ReceiveMessage();
    }

    class NetBufferReader
    {
        MemoryStream m_stream = null;
        BinaryReader m_reader = null;

        ushort m_dataLength;

        public NetBufferReader(byte[] data)
        {
            if (data != null)
            {
                m_stream = new MemoryStream(data);
                m_reader = new BinaryReader(m_stream);

                m_dataLength = ReadUShort();
            }
        }

        public byte ReadByte()
        {
            return m_reader.ReadByte();
        }

        public int ReadInt()
        {
            return m_reader.ReadInt32();
        }

        public uint ReadUInt()
        {
            return m_reader.ReadUInt32();
        }

        public short ReadShort()
        {
            return m_reader.ReadInt16();
        }

        public ushort ReadUShort()
        {
            return m_reader.ReadUInt16();
        }

        public long ReadLong()
        {
            return m_reader.ReadInt64();
        }

        public ulong ReadULong()
        {
            return m_reader.ReadUInt64();
        }

        public float ReadFloat()
        {
            byte[] temp = BitConverter.GetBytes(m_reader.ReadSingle());
            Array.Reverse(temp);
            return BitConverter.ToSingle(temp, 0);
        }

        public double ReadDouble()
        {
            byte[] temp = BitConverter.GetBytes(m_reader.ReadDouble());
            Array.Reverse(temp);
            return BitConverter.ToDouble(temp, 0);
        }

        public string ReadString()
        {
            ushort len = ReadUShort();
            byte[] buffer = new byte[len];
            buffer = m_reader.ReadBytes(len);
            return Encoding.UTF8.GetString(buffer);
        }

        public byte[] ReadBytes()
        {
            int len = ReadInt();
            return m_reader.ReadBytes(len);
        }

        public void Close()
        {
            if (m_reader != null)
            {
                m_reader.Close();
            }
            if (m_stream != null)
            {
                m_stream.Close();
            }
            m_reader = null;
            m_stream = null;
        }
    }

    class NetBufferWriter
    {
        MemoryStream m_stream = null;
        BinaryWriter m_writer = null;

        int m_finishLength;
        public int finishLength
        {
            get { return m_finishLength; }
        }

        public NetBufferWriter()
        {
            m_finishLength = 0;
            m_stream = new MemoryStream();
            m_writer = new BinaryWriter(m_stream);
        }

        public void WriteByte(byte v)
        {
            m_writer.Write(v);
        }

        public void WriteInt(int v)
        {
            m_writer.Write(v);
        }

        public void WriteUInt(uint v)
        {
            m_writer.Write(v);
        }

        public void WriteShort(short v)
        {
            m_writer.Write(v);
        }

        public void WriteUShort(ushort v)
        {
            m_writer.Write(v);
        }

        public void WriteLong(long v)
        {
            m_writer.Write(v);
        }

        public void WriteULong(ulong v)
        {
            m_writer.Write(v);
        }

        public void WriteFloat(float v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            m_writer.Write(BitConverter.ToSingle(temp, 0));
        }

        public void WriteDouble(double v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            m_writer.Write(BitConverter.ToDouble(temp, 0));
        }

        public void WriteString(string v)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(v);
            m_writer.Write((ushort)bytes.Length);
            m_writer.Write(bytes);
        }

        public void WriteBytes(byte[] v)
        {
            m_writer.Write(v.Length);
            m_writer.Write(v);
        }

        public byte[] ToBytes()
        {
            m_writer.Flush();
            return m_stream.ToArray();
        }

        public void Close()
        {
            m_writer.Close();
            m_stream.Close();
            m_writer = null;
            m_stream = null;
        }

        /// <summary>
        /// Encapsulate the written data stream into a new data stream (existing data lenght + existing data)
        /// Data conversion, network transmission requires two parts of data, one is the data length, the other is the main data
        /// </summary>
        public byte[] Finish()
        {
            byte[] message = ToBytes();
            MemoryStream ms = new MemoryStream();
            ms.Position = 0;
            BinaryWriter writer = new BinaryWriter(ms);
            writer.Write((ushort)message.Length);
            writer.Write(message);
            writer.Flush();
            byte[] result = ms.ToArray();
            m_finishLength = result.Length;
            return result;
        }
    }
}
