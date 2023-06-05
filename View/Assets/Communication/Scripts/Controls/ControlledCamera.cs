using Communication.Scripts.Components;
using Communication.Scripts.TDO;
using UnityEngine;

namespace Communication.Scripts.Controls
{
  public class ControlledCamera : MonoBehaviour
  {
    [SerializeField] public float speed;
      
    [SerializeField] public float left;
    [SerializeField] public float right;
    [SerializeField] public float up;
    [SerializeField] public float down;

    private void OnEnable() => 
      HeadControl.Instance.SetupCamera(GetComponent<Camera>(), new ControlledCameraData(speed, left, right, up, down));
  }
}