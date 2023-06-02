using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using _Scripts.Communication.Enum;
using _Scripts.Communication.TDO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cursor = _Scripts.Communication.Controls.Cursor;

namespace _Scripts.Communication.Components
{
  public class HeadControl : MonoBehaviour
  {
    [SerializeField] private TMP_Text ipAddress;
    [SerializeField] private Cursor cursor;

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
    }

    private void Start()
    {
      Communicator.Start();
      ipAddress.text = Communicator.GetLocalIPAddress();

      Communicator.Started += () => StartCoroutine(Communicator.SustainPool());
      Communicator.MessageReceived += ProcessMessage;
    }

    public void SetupCamera(Camera camera, ControlledCameraData controlledCameraData) => 
      cursor.SetupCamera(camera, controlledCameraData);

    public void Select() => 
      cursor.Raycast();
    
    private void ProcessMessage(string json)
    {
      var signal = JsonUtility.FromJson<ControllerSignal>(json);

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

    private void OnDestroy() => 
      Communicator.Server.Stop();
  }
}