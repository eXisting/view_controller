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
    public event Action<string> Closed;
    
    [SerializeField] private GameObject messageCard;
    [SerializeField] private TMP_Text userName;
    [SerializeField] private Button back;
    [SerializeField] private VerticalUnlimitedScroller scroll;

    private void Awake()
    {
      back.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
      Close();
    }

    public void Open(string userName)
    {
      this.userName.text = userName;
      scroll.Generate(messageCard, HeadControl.Instance.MessagesBank[userName].Count, SetupCard);

      gameObject.SetActive(true);
    }

    private void SetupCard(int index, ICell card)
    {
      (card as MessageCard)?.Setup(HeadControl.Instance.MessagesBank[userName.text][index], index);
    }

    private void Close()
    {
      scroll.Clear();
      gameObject.SetActive(false);
      Closed?.Invoke(this.userName.text);
    }
  }
}