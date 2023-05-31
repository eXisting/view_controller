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
    [SerializeField] private TMP_InputField ipAddress;
    [SerializeField] private Button connect;

    [SerializeField] private Joystick joystick;
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
      ipAddress.text = GetLocalIPAddress();
      connect.onClick.AddListener(Connect2Server);

      Communicator.Connected += () => StartCoroutine(Communicator.SustainPool());
      Communicator.MessageReceived += ProcessMessage;
    }

    // private void Update()
    // {
    //   cursor.direction = joystick.Direction;
    // }
    
    public void SetupCamera(Camera camera, RotationRestrictions rotationRestrictions) => 
      cursor.SetupCamera(camera, rotationRestrictions);

    public void Select() => 
      cursor.Raycast();

    private string GetLocalIPAddress()
    {
      var ipAddress = "";
      var interfaces = NetworkInterface.GetAllNetworkInterfaces();

      foreach (var iface in interfaces)
      {
        // Consider only the interfaces that are up and connected to a network
        if (iface.OperationalStatus == OperationalStatus.Up &&
            iface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
        {
          var addresses = iface.GetIPProperties().UnicastAddresses;
          foreach (var address in addresses)
          {
            // Consider only IPv4 addresses
            if (address.Address.AddressFamily != AddressFamily.InterNetwork)
              continue;

            ipAddress = address.Address.ToString();
            break;
          }
        }

        if (!string.IsNullOrEmpty(ipAddress))
          break;
      }

      return ipAddress;
    }
    
    private void ProcessMessage(string json)
    {
      var signal = JsonUtility.FromJson<ControllerSignal>(json);

      switch (signal.Operation)
      {
        case ControllerOperation.CursorMode:
          //nativeInput.SetActive(false);
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

    private void Connect2Server() => 
      Communicator.Connect(ipAddress.text);

    private void OnDestroy() => 
      Communicator.Client.Stop();
  }
}