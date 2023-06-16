using Communication.Scripts.Components;
using Communication.Scripts.DTO;
using Communication.Scripts.TDO;
using UnityEngine;

namespace Communication.Scripts.Controls
{
  public class ControlledCamera : MonoBehaviour
  {
    [SerializeField, Tooltip("Determines camera speed")] public float speed;

    [SerializeField, Tooltip("Allow camera to move without limits, unless you block one of axis")] 
    public bool freeCamera;
    [SerializeField, Tooltip("Blocks horizontal axis (right or left rotation suspended)")] 
    public bool blockHorizontal;
    [SerializeField, Tooltip("Blocks vertical axis (up or bo down rotation suspended)")]
    public bool blockVertical;
    
    [SerializeField, Range(-360f, 360f), Tooltip("Leftmost rotation value")]
    public float leftLimit;
    [SerializeField, Range(-360f, 360f), Tooltip("Rightmost rotation value")]
    public float rightLimit;
    [SerializeField, Range(-360f, 360f), Tooltip("Maximum turn up value")]
    public float topLimit;
    [SerializeField, Range(-360f, 360f), Tooltip("Maximum downward rotation value")]
    public float bottomLimit;

    private void OnEnable()
    {
      HeadControl.Instance.SetupCamera(GetComponent<Camera>(),
        new ControlledCameraData(speed,
          freeCamera, blockHorizontal, blockVertical,
          leftLimit < 0 ? 360 + leftLimit : leftLimit,
          rightLimit < 0 ? 360 + rightLimit : rightLimit,
          topLimit < 0 ? 360 + topLimit : topLimit,
          bottomLimit < 0 ? 360 + bottomLimit : bottomLimit));
    }
  }
}