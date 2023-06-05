using System;
using Communication.Scripts.Enum;
using Communication.Scripts.TDO;
using UnityEngine;

namespace Communication.Scripts.Components
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
      
      HeadControl.Instance.Communicator.Send(_signal);
    }
  }
}