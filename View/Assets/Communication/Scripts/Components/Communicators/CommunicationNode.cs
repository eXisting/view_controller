using System;
using Communication.Scripts.DTO;
using UnityEngine;

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

      _signal = new ViewSignal(viewSignal.operation,
        viewSignal.userName,
        viewSignal.message,
        _dateTime,
        viewSignal.videoId,
        viewSignal.loopVideo,
        viewSignal.muteVideo,
        viewSignal.subtitles);
      
      HeadControl.Instance.Communicator.Send(_signal);
    }
  }
}