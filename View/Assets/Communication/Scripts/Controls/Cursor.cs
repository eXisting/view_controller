using System;
using Communication.Scripts.DTO;
using Communication.Scripts.Enum;
using Communication.Scripts.TDO;
using UnityEngine;

namespace Communication.Scripts.Controls
{
  public class Cursor : MonoBehaviour
  {
    private const float CursorSpeedScale = 100f;
    private const float CameraSpeedScale = 0.2f;
    private const float RotationError = 2f;
    
    [SerializeField] private float cursorSpeed;
    [SerializeField] private RectTransform cursor;

    [SerializeField] private RaycastType raycastType;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask targetLayer;

    [HideInInspector] public Vector2 direction;

    private Camera _camera;
    private ControlledCameraData _camData;

    private float _cursorDimension;

    private void Start()
    {
      _cursorDimension = cursor.sizeDelta.x;
      
      cursorSpeed *= CursorSpeedScale;
    }

    private void Update()
    {
      if (direction == Vector2.zero)
        return;

      if (transform.localPosition.x + _cursorDimension / 2 > transform.parent.position.x && direction.x > 0
          || -transform.localPosition.x + _cursorDimension / 2 > transform.parent.position.x && direction.x < 0)
      {
        var horizontalRotation = _camera.transform.localEulerAngles.y;
        
        var leftLimitReached = _camData.right < _camData.left
          ? horizontalRotation > _camData.right + RotationError ? horizontalRotation < _camData.left : horizontalRotation > _camData.left
          : horizontalRotation < _camData.left;
        
        var rightLimitReached = _camData.right < _camData.left
          ? horizontalRotation < _camData.left - RotationError ? horizontalRotation > _camData.right : horizontalRotation < _camData.right
          : horizontalRotation > _camData.right;

        if (!_camData.blockHorizontal && (_camData.freeCamera || !rightLimitReached && direction.x > 0 || !leftLimitReached && direction.x < 0))
        {
          var rotation = Quaternion.Euler(0f, direction.x * _camData.speed * CameraSpeedScale, 0f);
          _camera!.transform.rotation = rotation * _camera!.transform.rotation;
        }

        direction.x = 0;
      }
      
      if (transform.localPosition.y + _cursorDimension / 2 > transform.parent.position.y && direction.y > 0
          || -transform.localPosition.y + _cursorDimension / 2 > transform.parent.position.y && direction.y < 0)
      {
        var verticalRotation = _camera.transform.localEulerAngles.x;
        
        var topLimitReached = _camData.up > _camData.down
          ? verticalRotation > _camData.down + RotationError ? verticalRotation < _camData.up : verticalRotation > _camData.up
          : verticalRotation < _camData.up;
      
        var downLimitReached = _camData.up > _camData.down
          ? verticalRotation < _camData.up - RotationError ? verticalRotation > _camData.down : verticalRotation < _camData.down
          : verticalRotation > _camData.down;

        if (!_camData.blockVertical && (_camData.freeCamera || !downLimitReached && direction.y < 0 || !topLimitReached && direction.y > 0))
        {
          var rotation = Quaternion.Euler(-direction.y * _camData.speed * CameraSpeedScale, 0f, 0f);
          _camera!.transform.rotation *= rotation;
        }

        direction.y = 0;
      }

      transform.Translate(direction * (cursorSpeed * Time.deltaTime));
    }

    public void SetupCamera(Camera camera, ControlledCameraData camData)
    {
      _camera = camera;
      _camData = camData;
    }

    public void Raycast()
    {
      var ray = _camera.ScreenPointToRay(transform.position);

      RaycastHit hit;
      switch (raycastType)
      {
        case RaycastType.Sphere:
          Physics.SphereCast(ray, radius, out hit, Mathf.Infinity, targetLayer);
          break;
        case RaycastType.Ray:
          Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      if (hit.collider == null)
        return;
      
      var fsm = hit.collider.gameObject.GetComponent<PlayMakerFSM>();

      var events = fsm.FsmEvents;

      foreach (var fsmEvent in events)
      {
        if (fsmEvent.IsMouseEvent)
          fsm.SendEvent(fsmEvent.Name);
      }
      
      // foreach (var fsmState in fsm.FsmStates)
      // {
      //   if (!string.Equals(fsmState.Name, fsm.ActiveStateName))
      //     return;
      //   
      //   foreach (var fsmTransition in fsmState.Transitions)
      //   {
      //     var fsmEvent = fsmTransition.FsmEvent;
      //
      //     if (!fsmEvent.IsMouseEvent) 
      //       continue;
      //     
      //     fsm.SendEvent(fsmEvent.Name);
      //     return;
      //   }
      // }

      Debug.Log("Hit object: " + hit.collider.gameObject.name);
    }
  }
}