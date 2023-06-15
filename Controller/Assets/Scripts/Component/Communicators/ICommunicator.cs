using System;
using System.Collections.Generic;
using DTO;

namespace Component.Communicators
{
  public interface ICommunicator
  {
    event Action<ViewSignal> MessageReceived;
    
    void Start(string ipAddress = default);
    
    void Send(ControllerSignal signal);

    void ProcessSignal(string json);
  }
}