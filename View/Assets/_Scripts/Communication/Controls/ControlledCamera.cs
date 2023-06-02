using _Scripts.Communication.Components;
using _Scripts.Communication.TDO;
using UnityEngine;

namespace _Scripts.Communication.Controls
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