using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using DTO;
using Enum;
using LiteNetLib;
using LiteNetLib.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Component.Communicators
{
  public class Client : MonoBehaviour, ICommunicator
  {
    public event Action<ViewSignal> MessageReceived;
    
    public Stack<ViewSignal> Calls => _calls;
    public Dictionary<string, List<MessageData>> MessagesBank => _messagesBank;
    
    
    private readonly Stack<ViewSignal> _calls = new();
    private Dictionary<string, List<MessageData>> _messagesBank = new();
    
    private NetManager _net;

    private void Awake()
    {
      _net = new NetManager(new ClientListener(this));

      _net.Start();
    }

    private void Start()
    {
      Debug.Log("Communicator: CLIENT");
      
      if (!string.IsNullOrEmpty(BlackBox.MessagesJson))
        _messagesBank = JsonConvert.DeserializeObject<Dictionary<string, List<MessageData>>>(BlackBox.MessagesJson);
    }
    
    private void OnDestroy() =>
      _net.Stop();

    public void Send(ControllerSignal signal)
    {
      var json = Newtonsoft.Json.JsonConvert.SerializeObject(signal);

      Debug.Log($"Message to send: {json}");
      
      if (_net is not { FirstPeer: not null } ||
          _net.FirstPeer.ConnectionState != ConnectionState.Connected)
        return;

      var writer = new NetDataWriter();
      writer.Put(json);
            
      _net.FirstPeer.Send(writer, DeliveryMethod.ReliableOrdered);
    }

    public void Start(string ipAddress)
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

    public void ProcessSignal(string json)
    {
      var signal = JsonConvert.DeserializeObject<ViewSignal>(json);

      switch (signal.Operation)
      {
        case ViewOperation.Message:
          if (_messagesBank.TryGetValue(signal.UserName, out var list))
          {
            list.Add(new MessageData(signal.UserName, signal.Message, signal.DateTime));
            BlackBox.SaveMessages(_messagesBank);
            break;
          }
          
          _messagesBank.Add(signal.UserName, new List<MessageData> { new(signal.UserName, signal.Message, signal.DateTime) });
          BlackBox.SaveMessages(_messagesBank);
          break;
        
        case ViewOperation.Call:
          _calls.Push(signal);
          break;
        
        default:
          Debug.LogError("Signal operation is unidentified");
          break;
      }
      
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