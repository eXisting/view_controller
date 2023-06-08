using System;
using System.Linq;
using Component;
using Component.Communicators;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

namespace UI.Messages
{
  public class MessagesPopup : MonoBehaviour
  {
    public event Action<int> Closed;
    
    [SerializeField] private GameObject messageCard;
    [SerializeField] private TMP_Text userName;
    
    [SerializeField] private VerticalUnlimitedScroller scroll;

    private int _index;

    private void OnDisable()
    {
      Close();
    }

    public void Open(string userName)
    {
      this.userName.text = userName;
      scroll.Generate(messageCard, HeadControl.Instance.Communicator.MessagesBank[userName].Count, SetupCard);

      gameObject.SetActive(true);
    }

    private void SetupCard(int index, ICell card)
    {
      _index = index;
      
      (card as MessageCard)?.Setup(HeadControl.Instance.Communicator.MessagesBank[userName.text][index], index);
    }

    private void Close()
    {
      scroll.Clear();
      gameObject.SetActive(false);
      Closed?.Invoke(_index);
    }
  }
}