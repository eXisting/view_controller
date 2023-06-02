using System;
using _Scripts.Communication.Enum;
using _Scripts.Communication.TDO;
using UnityEngine;

namespace _Scripts.Communication.Components
{
  public class CommunicationNode : MonoBehaviour
  {
    private ViewSignal _signal;
    private DateTime _dateTime;

    private bool _activated;

    public void Communicate(ViewOperation operation, string userName, string message, string videoId)
    {
      Debug.Log($"Communicate from node: {gameObject.name}");
      
      if (_activated)
        return;
      _activated = true;
      
      _dateTime = DateTime.Now;
      
      _signal = new ViewSignal(operation, userName, message, _dateTime, videoId);
      
      Communicator.ToController(_signal);
    }
  }
}