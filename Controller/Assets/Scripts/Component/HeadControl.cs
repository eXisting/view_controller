using System;
using System.Collections;
using System.Collections.Generic;
using Component.Communicators;
using DTO;
using Enum;
using Newtonsoft.Json;
using Screen;
using UI.Messages;
using UnityEngine;
using UnityEngine.Serialization;

namespace Component
{
  public class HeadControl : MonoBehaviour
  {
    [Header("Setup")]
    [SerializeField] public CommunicatorType communicatorType;
    [SerializeField] public bool testingMode;
    
    [Header("Embedded"), Space(20)]
    [SerializeField] private Client client;
    [SerializeField] private Server server;
    
    [SerializeField] private List<Page> screens;
    [SerializeField] private GameObject messagesDisplay;
    [SerializeField] private MessagesList messagesList;
    [SerializeField] private GameObject callNotification;
    [SerializeField] private MessageNotification messageNotification;

    [HideInInspector] public ICommunicator Communicator;
    
    public readonly Stack<ViewSignal> Calls = new();
    public Dictionary<string, List<MessageData>> MessagesBank = new();

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
      if (!string.IsNullOrEmpty(BlackBox.MessagesJson))
        MessagesBank = JsonConvert.DeserializeObject<Dictionary<string, List<MessageData>>>(BlackBox.MessagesJson);
      
      Navigator.Configure(screens);
      Navigator.Open(Enum.Screen.Sync);
      
      Communicator.MessageReceived += ProcessMessage;
      messageNotification.Clicked += OpenMessage;
    }

    private void ProcessMessage(ViewSignal signal)
    {
      switch (signal.Operation)
      {
        case ViewOperation.Message:
          var messageData = new MessageData(signal.UserName, signal.Message, signal.DateTime);

          if (MessagesBank.TryGetValue(signal.UserName, out var list))
            list.Add(messageData);
          else
            MessagesBank.Add(signal.UserName, new List<MessageData> { messageData });

          BlackBox.SaveMessages(MessagesBank);
          
          NotifyMessage(messageData);
          break;
        
        case ViewOperation.Call:
          Calls.Push(signal);
          
          NotifyCall();
          break;
        
        default:
          Debug.LogError("Operation is undefined");
          break;
      }
    }

    private void NotifyMessage(MessageData messageData)
    {
      if (messagesDisplay.activeInHierarchy)
        return;
      
      messageNotification.Show(messageData);
    }
    
    private void OpenMessage(string userName)
    {
      var page = Navigator.Open(Enum.Screen.Main);
      var mainPage = (MainScreen)page;
      
      mainPage.ChangeDisplay(Feature.Message);
      messagesList.Open(userName);
    }

    private void NotifyCall()
    {
      callNotification.gameObject.SetActive(true);
    }
    
    public void Test()
    {
      ReceiveMessage();
    }
    
    private IEnumerator TestCoroutine()
    {
      yield return new WaitForSeconds(3);
    }

    private void ReceiveCall()
    {
      var signal = new ViewSignal(ViewOperation.Call,
        "Clement",
        default,
        DateTime.Now,
        "Clem_Call_Source",
        true,
        true,
        new List<SubtitlePart>
        {
          new("Hi",
            1f,
            2f)
        });

      var json = JsonConvert.SerializeObject(signal);

      Communicator.ProcessSignal(json);
    }
    
    private void ReceiveMessage()
    {
      var signal = new ViewSignal(ViewOperation.Message,
        "Clement",
        "Something is wrong with messages. Something is wrong with messages. Something is wrong with messages. Something is wrong with messages. Something is wrong with messages. When message is too long it isnt properly visible. A little bit more.",
        DateTime.Now);

      var json = JsonConvert.SerializeObject(signal);

      Communicator.ProcessSignal(json);
    }
  }
}