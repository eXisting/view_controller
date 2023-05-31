using System.Collections.Generic;
using Enum;
using Screen;
using Tdo;
using UI;
using UnityEngine;

namespace Component
{
  public class HeadControl : MonoBehaviour
  {
    [SerializeField] private List<Page> screens;
    [SerializeField] private AudioClip messageRingtone;

    [SerializeField] private GameObject callNotification;
    [SerializeField] private Bubble messageBubble;

    
    private void Start()
    {
      Navigator.Configure(screens);
      Navigator.Open(Enum.Screen.Sync);

      Communicator.ServerStarted += SustainServer;
      Communicator.MessageReceived += ProcessMessage;
    }

    private void OnDestroy()
    {
      Communicator.ServerStarted -= SustainServer;
      Communicator.Stop();
    }

    private void SustainServer()
    {
      StartCoroutine(Communicator.SustainConnection());
    }
    
    private void ProcessMessage(ControllerSignal signal)
    {
      switch (signal.Operation)
      {
        case ControllerOperation.Message:
          NotifyMessage();
          break;
        case ControllerOperation.Call:
          StartCoroutine(nameof(NotifyCall));
          break;
        default:
          Debug.LogError("Operation is undefined");
          break;
      }
    }

    private void NotifyMessage()
    {
      Handheld.Vibrate();
      AudioSource.PlayClipAtPoint(messageRingtone, transform.position);
      
      messageBubble.Increase();
    }

    private void NotifyCall()
    {
      callNotification.gameObject.SetActive(true);
    }
  }
}