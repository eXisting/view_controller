using System.Collections.Generic;
using System.Linq;
using Component;
using DTO;
using UnityEngine;
using UnityEngine.Serialization;
using UnlimitedScrollUI;

namespace UI.Messages
{
  public class UsersList : MonoBehaviour
  {
    [SerializeField] private GameObject userCard;
    [SerializeField] private VerticalUnlimitedScroller scroll;

    [FormerlySerializedAs("messagesPopup")] [SerializeField] private MessagesList messagesList;

    private readonly List<(MessageData data, bool read)> _lastMessages = new();

    private void OnEnable()
    {
      foreach (var messagePair in HeadControl.Instance.MessagesBank)
      {
        var firstOrDefaultUnread = messagePair.Value.FirstOrDefault(x => !x.Read);

        _lastMessages.Add((messagePair.Value.Last(), string.IsNullOrEmpty(firstOrDefaultUnread.UserName)));
      }
      
      Generate();

      messagesList.Closed += Refresh;
    }

    public void Refresh()
    {
      _lastMessages.Clear();
      Generate();
    }
    
    private void Refresh(string userName)
    {
      var index = _lastMessages.FindIndex(x => x.data.UserName == userName);

      if (index != -1)
      {
        var firstOrDefaultUnread = HeadControl.Instance.MessagesBank[_lastMessages[index].data.UserName]
          .FirstOrDefault(x => !x.Read);

        _lastMessages[index] = (HeadControl.Instance.MessagesBank[_lastMessages[index].data.UserName].Last(),
          string.IsNullOrEmpty(firstOrDefaultUnread.UserName));
      }

      Generate();
    }

    private void Generate()
    {
      scroll.Clear();
      scroll.Generate(userCard, HeadControl.Instance.MessagesBank.Count, SetupCard);
    }

    private void SetupCard(int index, ICell card)
    {
      var userCard = card as UserCard;
      if (userCard == null) 
        return;
      
      userCard.Setup(_lastMessages[index].data, _lastMessages[index].read);

      userCard.Pressed += messagesList.Open;
    }
  }
}