using Component;
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

    private void OnEnable() => 
      Communicator.ToView(_modeSignal);

    private void Update()
    {
      if (joystick.Direction == Vector2.zero)
        return;
      
      _moveSignal.Direction = joystick.Direction;
      Communicator.ToView(_moveSignal);
    }
  }
}