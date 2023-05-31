using _Scripts.Communication.TDO;
using UnityEngine;

namespace _Scripts.Communication.Controls
{
  public class Cursor : MonoBehaviour
  {
    private const float CursorSpeedScale = 100f;
    private const float CameraSpeedScale = 0.2f;
    
    [SerializeField] private float cursorSpeed;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private LayerMask targetLayer;
    
    [HideInInspector] public Vector2 direction;

    private Camera camera;
    private RotationRestrictions restrictions;

    private float cursorDimension;

    private void Start()
    {
      cursorDimension = cursor.sizeDelta.x;
      
      cursorSpeed *= CursorSpeedScale;
      cameraSpeed *= CameraSpeedScale;
    }

    private void Update()
    {
      if (direction == Vector2.zero)
        return;

      if (transform.localPosition.x + cursorDimension / 2 > transform.parent.position.x && direction.x > 0
          || -transform.localPosition.x + cursorDimension / 2 > transform.parent.position.x && direction.x < 0)
      {
        if (camera.transform.localEulerAngles.y < restrictions.right && direction.x > 0
            || camera.transform.localEulerAngles.y > restrictions.left && direction.x < 0)
        {
          var rotation = Quaternion.Euler(0f, direction.x * cameraSpeed, 0f);
          camera!.transform.rotation = rotation * camera!.transform.rotation;
        }

        direction.x = 0;
      }

      if (transform.localPosition.y + cursorDimension / 2 > transform.parent.position.y && direction.y > 0
          || -transform.localPosition.y + cursorDimension / 2 > transform.parent.position.y && direction.y < 0)
      {
        if ((camera.transform.localEulerAngles.x < restrictions.down && direction.y < 0)
            || (camera.transform.localEulerAngles.x > restrictions.up && direction.y > 0))
        {
          var rotation = Quaternion.Euler(-direction.y / cameraSpeed, 0f, 0f);
          camera!.transform.rotation *= rotation;
        }

        direction.y = 0;
      }

      transform.Translate(direction * (cursorSpeed * Time.deltaTime));
    }

    public void SetupCamera(Camera camera, RotationRestrictions restrictions)
    {
      this.camera = camera;
      this.restrictions = restrictions;
    }

    public void Raycast()
    {
      var ray = camera.ScreenPointToRay(transform.position);

      if (!Physics.SphereCast(ray, .02f, out var hit, Mathf.Infinity, targetLayer)) 
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