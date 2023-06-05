using System.Collections.Generic;
using Component.Communicators;
using DTO;
using Enum;
using Screen;
using UnityEngine;

namespace Component
{
  public class HeadControl : MonoBehaviour
  {
    [Header("Communication")]
    [SerializeField] public CommunicatorType communicatorType;
    [SerializeField] private Client client;
    [SerializeField] private Server server;
    
    [Space(10)]
    [SerializeField] private List<Page> screens;
    
    [SerializeField] private AudioClip messageRingtone;
    [SerializeField] private GameObject callNotification;

    public ICommunicator Communicator;

    private static HeadControl _instance;

    public static HeadControl Instance
    {
      get
      {
        if (_instance != null) 
          return _instance;
        
        _instance = FindObjectOfType<HeadControl>();

        if (_instance != null) 
          return _instance;
        
        var singletonObject = new GameObject(nameof(HeadControl));
        _instance = singletonObject.AddComponent<HeadControl>();
        DontDestroyOnLoad(singletonObject);

        return _instance;
      }
    }

    private void Awake()
    {
      if (_instance != null && _instance != this) 
        Destroy(gameObject);

      if (communicatorType == CommunicatorType.Client)
        Communicator = Instantiate(client, transform);
      else
        Communicator = Instantiate(server, transform);
    }

    private void Start()
    {
      Navigator.Configure(screens);
      Navigator.Open(Enum.Screen.Sync);
      
      Communicator.MessageReceived += ProcessMessage;
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
    }

    private void NotifyCall()
    {
      callNotification.gameObject.SetActive(true);
    }
  }
}