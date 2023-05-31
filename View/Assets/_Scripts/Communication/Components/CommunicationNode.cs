using System;
using _Scripts.Communication.CustomEditorScripts;
using _Scripts.Communication.Enum;
using _Scripts.Communication.TDO;
using UnityEngine;

namespace _Scripts.Communication.Components
{
  public class CommunicationNode : MonoBehaviour
  {
    [SerializeField] private ViewOperation operation;
    [SerializeField] private string userName;
    [SerializeField] private string message;
    //[DateTimeInput, SerializeField] private DateTimeSerializable date;
    [SerializeField] private string videoName;

    private ViewSignal signal;
    private DateTime dateTime;

    private bool activated;

    private void Start()
    {
      dateTime = DateTime.Now;
      
      signal = new ViewSignal(operation, userName, message, dateTime, videoName);
    }

    public void Communicate()
    {
      if (activated)
        return;
      activated = true;
      
      Communicator.ToController(signal);
    }
  }
}