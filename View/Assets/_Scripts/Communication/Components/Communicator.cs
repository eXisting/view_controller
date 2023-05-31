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
        public static event Action Connected;
        internal static event Action<string> MessageReceived; 
        
        internal static readonly NetManager Client = new(new ClientListener());

        internal static void Connect(string ipAddress)
        {
            Client.Start();
            Client.Connect(ipAddress, 9050, "YouCantConnectWithoutKey");
            
            Connected?.Invoke();
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

                Client.PollEvents();
            }
        }

        internal static void ToController(ViewSignal signal)
        {
            var json = JsonUtility.ToJson(signal);
            
            if (Client is not { FirstPeer: not null } ||
                Client.FirstPeer.ConnectionState != ConnectionState.Connected)
                return;

            var writer = new NetDataWriter();
            writer.Put(json);
            Client.FirstPeer.Send(writer, DeliveryMethod.ReliableOrdered);
        }
    }

    internal class ClientListener : INetEventListener
    {
        public void OnPeerConnected(NetPeer peer) => 
            Debug.Log("Client connected: " + peer.EndPoint);

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

            // You can send a response back to the client if needed
            // Example: peer.Send("Response", DeliveryMethod.ReliableOrdered);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, 
            UnconnectedMessageType messageType) { }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency) { }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            Debug.Log("Someone requested connection");
            
            var peer = request.AcceptIfKey("YouCantConnectWithoutKey");
            
            Debug.Log(peer == null ? "Successful connection" : "Connection failed");
        }
    }
}