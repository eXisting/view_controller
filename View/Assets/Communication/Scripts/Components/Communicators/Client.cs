using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using Communication.Scripts.DTO;
using Communication.Scripts.TDO;
using LiteNetLib;
using LiteNetLib.Utils;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Communication.Scripts.Components.Communicators
{
  public class Client : MonoBehaviour, ICommunicator
  {
    public event Action<ControllerSignal> MessageReceived;

    [SerializeField] private TMP_InputField ipAddress;
    [SerializeField] private Button connect;

    private NetManager _net;
    
    private void Awake()
    {
      _net = new NetManager(new ClientListener(this));

      _net.Start();
    }

    private void Start()
    {
      Debug.Log("Communicator: CLIENT");
      
      connect.onClick.AddListener( () => Connect(ipAddress.text));
    }

    private void OnDestroy()
    {
      _net.Stop();
    }

    public void Send(ViewSignal signal)
    {
      var json = JsonConvert.SerializeObject(signal);

      Debug.Log($"Message to send: {json}");
      
      if (_net is not { FirstPeer: not null } ||
          _net.FirstPeer.ConnectionState != ConnectionState.Connected)
        return;

      var writer = new NetDataWriter();
      writer.Put(json);
            
      _net.FirstPeer.Send(writer, DeliveryMethod.ReliableOrdered);
    }

    public void Connect(string ipAddress)
    {
      _net.Connect(ipAddress, 9050, "YouCantConnectWithoutKey");

      StartCoroutine(nameof(SustainPool));
    }

    internal IEnumerator SustainPool()
    {
      while (true)
      {
        yield return null;

        _net.PollEvents();
      }
    }

    internal void ProcessSignal(string json)
    {
      var signal = JsonConvert.DeserializeObject<ControllerSignal>(json);

      MessageReceived?.Invoke(signal);
    }
  }

  public class ClientListener : INetEventListener
  {
    private readonly Client _client;

    public ClientListener(Client client)
    {
      _client = client;
    }

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
            
      _client.ProcessSignal(receivedString);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, 
      UnconnectedMessageType messageType) { }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
      Debug.Log("Someone requested connection");
    }
  }
}