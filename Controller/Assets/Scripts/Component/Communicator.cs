using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using LiteNetLib.Utils;
using Tdo;
using UnityEngine;
using LiteNetLib;

namespace Component
{
  public static class Communicator
  {
    public static Stack<ControllerSignal> Signals = new();
    
    internal static event Action ServerStarted;
    internal static event Action<ControllerSignal> MessageReceived; 
    
    internal static NetPeer Peer;

    private static readonly NetManager Server;

    static Communicator()
    {
      Server = new NetManager(new ServerListener());
    }

    internal static void ToView(ViewSignal viewSignal)
    {
      var json = JsonUtility.ToJson(viewSignal);

      Debug.Log($"Message to send: {json}");
      
      if (Server is not { FirstPeer: not null } ||
          Peer.ConnectionState != ConnectionState.Connected)
        return;

      var writer = new NetDataWriter();
      writer.Put(json);
            
      Peer.Send(writer, DeliveryMethod.ReliableOrdered);
    }
    
    internal static void Start()
    {
      Server.Start(9050);
      ServerStarted?.Invoke();
    }
    
    internal static void Stop() => 
      Server.Stop();

    internal static IEnumerator SustainConnection()
    {
      while (true)
      {
        yield return null;

        Server.PollEvents();
      }
    }

    internal static void ProcessMessage(string json)
    {
      var signal = JsonUtility.FromJson<ControllerSignal>(json);
      Signals.Push(signal);

      MessageReceived?.Invoke(signal);
    }
  }

  public class ServerListener : INetEventListener
  {
    public void OnPeerConnected(NetPeer peer)
    {
      Communicator.Peer = peer;
      
      Debug.Log("Connected to server: " + peer.EndPoint);
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

      // You can send a response back to the client if needed
      // Example: peer.Send("Response", DeliveryMethod.ReliableOrdered);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, 
      UnconnectedMessageType messageType) { }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency) { }

    public void OnConnectionRequest(ConnectionRequest request)
    {
      Debug.Log("Someone requested connection " + request.RemoteEndPoint);
      
      var peer = request.AcceptIfKey("YouCantConnectWithoutKey");
      
      Debug.Log(peer == null ? "Connection failed" : $"Successful connection from: {request.RemoteEndPoint}");
    }
  }
}