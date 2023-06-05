using System;
using Component;
using Component.Communicators;
using DTO;
using Enum;
using UnityEngine;

namespace Display
{
  public class CameraControl : MonoBehaviour
  {
    [SerializeField] private FixedJoystick joystick;

    private readonly ControllerSignal _moveSignal = new(ControllerOperation.MoveCursor);
    private readonly ControllerSignal _modeSignal = new(ControllerOperation.CursorMode);

    private ICommunicator _communicator;
    
    private void Start()
    {
      _communicator = HeadControl.Instance.Communicator;
    }

    private void OnEnable() => 
      HeadControl.Instance.Communicator.Send(_modeSignal);

    private void Update()
    {
      if (joystick.Direction == Vector2.zero)
        return;
      
      _moveSignal.Direction = joystick.Direction;
      _communicator.Send(_moveSignal);
    }
  }
}