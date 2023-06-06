using System;
using System.Collections.Generic;
using Communication.Scripts.DTO;
using Communication.Scripts.Enum;
using Communication.Scripts.TDO;
using UnityEngine;
using Object = System.Object;

namespace Communication.Scripts.Components.Communicators
{
  public class CommunicationNode : MonoBehaviour
  {
    private ViewSignal _signal;
    private DateTime _dateTime;

    private bool _activated;

    public void Communicate(ViewSignalScriptable viewSignal)
    {
      Debug.Log($"Communicate from node: {gameObject.name}");
      
      if (_activated)
        return;
      _activated = true;
      
      _dateTime = DateTime.Now;

      _signal = new ViewSignal(viewSignal.operation, viewSignal.userName, viewSignal.message, _dateTime, viewSignal.videoId, viewSignal.subtitles);
      
      HeadControl.Instance.Communicator.Send(_signal);
    }
  }
}