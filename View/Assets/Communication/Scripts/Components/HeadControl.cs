using System;
using Communication.Scripts.Components.Communicators;
using Communication.Scripts.Enum;
using Communication.Scripts.TDO;
using UnityEngine;
using Cursor = _Scripts.Communication.Controls.Cursor;

namespace Communication.Scripts.Components
{
  public class HeadControl : MonoBehaviour
  {
    [SerializeField] private CommunicatorType communicatorType;
    [SerializeField] private Client client;
    [SerializeField] private Server server;
    
    [SerializeField] private Cursor cursor;

    public ICommunicator Communicator;
    
    private static HeadControl instance;

    public static HeadControl Instance
    {
      get
      {
        if (instance != null) 
          return instance;
        
        instance = FindObjectOfType<HeadControl>();

        if (instance != null) 
          return instance;
        
        var singletonObject = new GameObject(nameof(HeadControl));
        instance = singletonObject.AddComponent<HeadControl>();
        DontDestroyOnLoad(singletonObject);

        return instance;
      }
    }

    private void Awake()
    {
      if (instance != null && instance != this) 
        Destroy(gameObject);
      
      if (communicatorType == CommunicatorType.Client)
        Communicator = Instantiate(client, transform);
      else
        Communicator = Instantiate(server, transform);
    }

    private void Start()
    {
      Communicator.MessageReceived += ProcessMessage;
    }

    public void SetupCamera(Camera camera, ControlledCameraData controlledCameraData) => 
      cursor.SetupCamera(camera, controlledCameraData);

    public void Select() => 
      cursor.Raycast();
    
    private void ProcessMessage(ControllerSignal signal)
    {
      switch (signal.Operation)
      {
        case ControllerOperation.CursorMode:
          break;
        case ControllerOperation.MoveCursor:
          cursor.direction = signal.Direction;
          break;
        case ControllerOperation.Select:
          cursor.Raycast();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}