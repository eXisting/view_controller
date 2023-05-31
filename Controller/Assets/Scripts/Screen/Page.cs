using UnityEngine;

namespace Screen
{
  public abstract class Page : MonoBehaviour
  {
    internal void Open() => 
      gameObject.SetActive(true);

    internal void Close() => 
      gameObject.SetActive(false);
  }
}