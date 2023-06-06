using System;
using System.Collections.Generic;
using DTO;

namespace Component.Communicators
{
  public interface ICommunicator
  {
    Stack<ViewSignal> Calls { get; }
    Dictionary<string, List<MessageData>> MessagesBank { get; }
    
    event Action<ViewSignal> MessageReceived;
    
    void Start(string ipAddress = default);
    
    void Send(ControllerSignal signal);

    void ProcessSignal(string json);
  }
}