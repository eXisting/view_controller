using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  public class Switcher : MonoBehaviour
  {
    [SerializeField] internal Image image;
    [SerializeField] internal Button button;
    [SerializeField] internal Sprite onLook;
    [SerializeField] internal Sprite offLook;
  }
}