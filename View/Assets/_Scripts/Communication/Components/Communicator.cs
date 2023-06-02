using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using _Scripts.Communication.TDO;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

namespace _Scripts.Communication.Components
{
    public static class Communicator
    {
        public static event Action Started;
        internal static event Action<string> MessageReceived;
        
        internal static NetPeer Peer;
        internal static readonly NetManager Server = new(new ServerListener());

        internal static void Start()
        {
            Server.Start(9050);
            
            Started?.Invoke();
        }
        
        internal static void ProcessMessage(string msg)
        {
            MessageReceived?.Invoke(msg);
        }

        internal static IEnumerator SustainPool()
        {
            while (true)
            {
                yield return null;

                Server.PollEvents();
            }
        }

        internal static void ToController(ViewSignal signal)
        {
            var json = JsonUtility.ToJson(signal);
            
            Debug.Log(json);
            
            if (Server is not { FirstPeer: not null } ||
                Peer.ConnectionState != ConnectionState.Connected)
                return;

            var writer = new NetDataWriter();
            writer.Put(json);
            Peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }
        
        internal static string GetLocalIPAddress()
        {
            var ipAddress = string.Empty;
      
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            if (socket.LocalEndPoint is IPEndPoint endPoint)
            {
                ipAddress = endPoint.Address.ToString();
            }
            return ipAddress;
        }
    }

    internal class ServerListener : INetEventListener
    {
        public void OnPeerConnected(NetPeer peer)
        {
            Communicator.Peer = peer;
      
            Debug.Log("Connected to server: " + peer.EndPoint);Debug.Log("Client connected: " + peer.EndPoint);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) =>
            Debug.Log("Client disconnected: " + peer.EndPoint + ", Reason: " + disconnectInfo.Reason);

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) =>
            Debug.Log($"Network error occurred: {socketError}");

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber,
            DeliveryMethod deliveryMethod)
        {
            var receivedString = reader.GetString();
            Debug.Log($"Message received: {receivedString}");
            
            Communicator.ProcessMessage(receivedString);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, 
            UnconnectedMessageType messageType) { }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency) { }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            Debug.Log("Someone requested connection");
            
            var peer = request.AcceptIfKey("YouCantConnectWithoutKey");
            
            Debug.Log(peer == null ? "Connection failed" : "Successful connection");
        }
    }
}