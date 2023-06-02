using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using DTO;
using Enum;
using LiteNetLib.Utils;
using UnityEngine;
using LiteNetLib;
using Newtonsoft.Json;

namespace Component
{
  public static class Communicator
  {
    public static readonly Stack<ViewSignal> Calls = new();
    public static Dictionary<string, List<MessageData>> MessagesBank = new();

    internal static event Action ServerConnected;
    internal static event Action<ViewSignal> MessageReceived; 
    
    private static readonly NetManager Client;

    static Communicator()
    {
      Client = new NetManager(new ClientListener());

      Client.Start();
    }

    internal static void ToView(ControllerSignal controllerSignal)
    {
      var json = JsonUtility.ToJson(controllerSignal);

      Debug.Log($"Message to send: {json}");
      
      if (Client is not { FirstPeer: not null } ||
          Client.FirstPeer.ConnectionState != ConnectionState.Connected)
        return;

      var writer = new NetDataWriter();
      writer.Put(json);
            
      Client.FirstPeer.Send(writer, DeliveryMethod.ReliableOrdered);
    }
    
    internal static void Connect(string ipAddress)
    {
      Client.Connect(ipAddress, 9050, "YouCantConnectWithoutKey");

      if (!string.IsNullOrEmpty(BlackBox.MessagesJson))
        MessagesBank = JsonConvert.DeserializeObject<Dictionary<string, List<MessageData>>>(BlackBox.MessagesJson);
      
      ServerConnected?.Invoke();
    }

    internal static void Stop() => 
      Client.Stop();

    internal static IEnumerator SustainConnection()
    {
      while (true)
      {
        yield return null;

        Client.PollEvents();
      }
    }

    internal static void ProcessSignal(string json)
    {
      var signal = JsonUtility.FromJson<ViewSignal>(json);

      switch (signal.Operation)
      {
        case ViewOperation.Message:
          if (MessagesBank.TryGetValue(signal.UserName, out var list))
          {
            list.Add(new MessageData(signal.UserName, signal.Message, signal.DateTime));
            break;
          }
          MessagesBank.Add(signal.UserName, new List<MessageData> { new(signal.UserName, signal.Message, signal.DateTime) });

          BlackBox.SaveMessages(MessagesBank);
          break;
        
        case ViewOperation.Call:
          Calls.Push(signal);
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
            
      Communicator.ProcessSignal(receivedString);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, 
      UnconnectedMessageType messageType) { }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
      Debug.Log($"Peer: {peer.EndPoint.Address}");
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
      Debug.Log("Someone requested connection");
    }
  }
}