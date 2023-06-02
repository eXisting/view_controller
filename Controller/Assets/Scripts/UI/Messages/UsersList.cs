using System;
using System.Collections.Generic;
using System.Linq;
using Component;
using DTO;
using UnityEngine;
using UnlimitedScrollUI;

namespace UI.Messages
{
  public class UsersList : MonoBehaviour
  {
    [SerializeField] private GameObject userCard;
    [SerializeField] private VerticalUnlimitedScroller scroll;

    [SerializeField] private MessagesPopup messagesPopup;

    private readonly List<(MessageData data, bool read)> _lastMessages = new();

    private void Awake()
    {
      foreach (var messagePair in Communicator.MessagesBank)
      {
        var firstOrDefaultUnread = messagePair.Value.FirstOrDefault(x => !x.Read);

        _lastMessages.Add((messagePair.Value.Last(), string.IsNullOrEmpty(firstOrDefaultUnread.UserName)));
      }
    }

    private void OnEnable()
    {
      Generate();

      messagesPopup.Closed += Refresh;
    }

    public void Refresh()
    {
      _lastMessages.Clear();
      Generate();
    }
    
    private void Refresh(int index)
    {
      var firstOrDefaultUnread = Communicator.MessagesBank[_lastMessages[index].data.UserName].FirstOrDefault(x => !x.Read);

      _lastMessages[index] = (Communicator.MessagesBank[_lastMessages[index].data.UserName].Last(),
        string.IsNullOrEmpty(firstOrDefaultUnread.UserName));
      
      Generate();
    }

    private void Generate()
    {
      scroll.Clear();
      scroll.Generate(userCard, Communicator.MessagesBank.Count, SetupCard);
    }

    private void SetupCard(int index, ICell card)
    {
      var userCard = card as UserCard;
      if (userCard == null) 
        return;
      
      userCard.Setup(_lastMessages[index].data, _lastMessages[index].read);

      userCard.Pressed += messagesPopup.Open;
    }
  }
}