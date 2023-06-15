using System.Collections.Generic;
using System.Linq;
using Component.Communicators;
using DTO;
using Enum;
using Newtonsoft.Json;
using Screen;
using UI.Messages;
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
    [SerializeField] private GameObject messagesDisplay;
    [SerializeField] private MessagesPopup messagesPopup;
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
      messagesPopup.Open(userName);
    }

    private void NotifyCall()
    {
      callNotification.gameObject.SetActive(true);
    }
  }
}