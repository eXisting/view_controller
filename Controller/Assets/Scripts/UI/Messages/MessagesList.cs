using System;
using System.Collections.Generic;
using Component;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Messages
{
  public class MessagesList : MonoBehaviour
  {
    public event Action<string> Closed;
    
    [SerializeField] private MessageCard messageCard;
    [SerializeField] private TMP_Text header;
    [SerializeField] private Button back;
    [SerializeField] private Transform content;

    private List<MessageCard> _messagesGO = new();
    
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
      header.text = userName;

      for (var index = 0; index < HeadControl.Instance.MessagesBank[userName].Count; index++)
      {
        var message = Instantiate(messageCard, content);
        message.Setup(HeadControl.Instance.MessagesBank[userName][index], index);
        
        _messagesGO.Add(message);
      }

      gameObject.SetActive(true);
    }

    private void Close()
    {
      foreach (var messageGO in _messagesGO) 
        Destroy(messageGO.gameObject);

      _messagesGO.Clear();
      
      gameObject.SetActive(false);
      Closed?.Invoke(header.text);
    }
  }
}