using System;
using Communication.Scripts.TDO;

namespace Communication.Scripts.Components.Communicators
{
  public interface ICommunicator
  {
    event Action<ControllerSignal> MessageReceived;
    
    void Connect(string ipAddress = default);
    
    void Send(ViewSignal signal);
  }
}