using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using DTO;
using LiteNetLib;
using LiteNetLib.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Component.Communicators
{
    public class Server : MonoBehaviour, ICommunicator
    {
        public event Action<ViewSignal> MessageReceived;

        private NetManager _net;
        internal NetPeer Peer;

        private void Awake()
        {
            _net = new(new ServerListener(this));
        }

        internal void Start()
        {
            _net.Start(9050);
            StartCoroutine(nameof(SustainPool));

            Debug.Log("Communicator: SERVER");
        }

        private void OnDestroy() =>
            _net.Stop();

        public void Start(string ipAddress)
        {
            
        }

        public void Send(ControllerSignal signal)
        {
            var json = JsonConvert.SerializeObject(signal);

            Debug.Log(json);

            if (_net is not { FirstPeer: not null } ||
                Peer.ConnectionState != ConnectionState.Connected)
                return;

            var writer = new NetDataWriter();
            writer.Put(json);
            Peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }
        
        public void ProcessSignal(string json)
        {
            var signal = JsonConvert.DeserializeObject<ViewSignal>(json, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            MessageReceived?.Invoke(signal);
        }

        internal IEnumerator SustainPool()
        {
            while (true)
            {
                yield return null;

                _net.PollEvents();
            }
        }

        internal string GetLocalIPAddress()
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
        private readonly Server _server;
        
        public ServerListener(Server server)
        {
            _server = server;
        }
        
        public void OnPeerConnected(NetPeer peer)
        {
            _server.Peer = peer;

            Debug.Log("Connected to server: " + peer.EndPoint);
            Debug.Log("Client connected: " + peer.EndPoint);
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

            _server.ProcessSignal(receivedString);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader,
            UnconnectedMessageType messageType)
        {
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            Debug.Log("Someone requested connection");

            var peer = request.AcceptIfKey("YouCantConnectWithoutKey");

            Debug.Log(peer == null ? "Connection failed" : "Successful connection");
        }
    }
}