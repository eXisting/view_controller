using System;
using System.Linq;
using Component;
using TMPro;
using UnityEngine;

namespace UI.Messages
{
  public class Bubble : MonoBehaviour
  {
    [SerializeField] private TMP_Text counter;

    private int _counter;

    private void Awake()
    {
      BlackBox.NewData += Refresh;
    }

    private void OnEnable()
    {
      Refresh();
    }

    public void Refresh()
    {
      _counter = 0;
      
      foreach (var unread in Communicator.MessagesBank.Select(messagePair => messagePair.Value.FindAll(x => !x.Read)))
        _counter += unread.Count;
      
      Show();
    }
    
    public void Show()
    {
      counter.text = _counter.ToString();
      gameObject.SetActive(_counter != 0);
    }
  }
}