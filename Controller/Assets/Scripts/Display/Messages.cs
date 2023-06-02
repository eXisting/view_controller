using Component;
using UI.Messages;
using UnityEngine;
using UnityEngine.UI;

namespace Display
{
  public class Messages : MonoBehaviour
  {
    [SerializeField] private UsersList list;
    [SerializeField] private Button clear;
    [SerializeField] private Bubble bubble;
    
    private void Start()
    {
      clear.onClick.AddListener(ClearAllMessages);
    }

    private void ClearAllMessages()
    {
      BlackBox.ClearMessages();
      Communicator.MessagesBank.Clear();
      list.Refresh();
      bubble.Refresh();
    }
  }
}