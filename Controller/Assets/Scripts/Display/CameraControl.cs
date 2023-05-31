using Component;
using Enum;
using Tdo;
using UnityEngine;

namespace Display
{
  public class CameraControl : MonoBehaviour
  {
    [SerializeField] private FixedJoystick joystick;

    private readonly ViewSignal _moveSignal = new(ViewOperation.MoveCursor);
    private readonly ViewSignal _modeSignal = new(ViewOperation.CursorMode);

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