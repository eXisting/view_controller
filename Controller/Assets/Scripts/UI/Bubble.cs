using TMPro;
using UnityEngine;

namespace UI
{
  public class Bubble : MonoBehaviour
  {
    [SerializeField] private TMP_Text counter;

    private int _counter;

    private void OnEnable() => 
      Show();

    private void Show()
    {
      gameObject.SetActive(_counter != 0);
      counter.text = _counter.ToString();
    }

    internal void Increase()
    {
      _counter++;
      Show();
    }

    internal void Decrease()
    {
      _counter--;
      Show();
    }
  }
}