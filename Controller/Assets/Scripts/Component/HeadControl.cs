using System.Collections.Generic;
using DTO;
using Enum;
using Screen;
using UI;
using UI.Messages;
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

      Communicator.ServerConnected += SustainServer;
      Communicator.MessageReceived += ProcessMessage;
    }

    private void OnDestroy()
    {
      Communicator.ServerConnected -= SustainServer;
      Communicator.Stop();
    }

    private void SustainServer()
    {
      StartCoroutine(Communicator.SustainConnection());
    }
    
    private void ProcessMessage(ViewSignal signal)
    {
      switch (signal.Operation)
      {
        case ViewOperation.Message:
          NotifyMessage();
          break;
        case ViewOperation.Call:
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