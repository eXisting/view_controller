using _Scripts.Communication.Components;
using _Scripts.Communication.TDO;
using UnityEngine;

namespace _Scripts.Communication.Controls
{
  public class ControlledCamera : MonoBehaviour
  {
    [SerializeField] public float left;
    [SerializeField] public float right;
    [SerializeField] public float up;
    [SerializeField] public float down;

    private void OnEnable() => 
      HeadControl.Instance.SetupCamera(GetComponent<Camera>(), new RotationRestrictions(left, right, up, down));
  }
}