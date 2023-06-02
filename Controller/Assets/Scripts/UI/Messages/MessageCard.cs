using System.Globalization;
using Component;
using DTO;
using TMPro;
using UnityEngine;
using UnlimitedScrollUI;

namespace UI.Messages
{
  public class MessageCard : MonoBehaviour, ICell
  {
    [SerializeField] private TMP_Text date;
    [SerializeField] private TMP_Text message;

    [SerializeField] private GameObject read;

    private string _userName;
    private int _index;
    
    public void Setup(MessageData data, int index)
    {
      _userName = data.UserName;
      _index = index;
      
      date.text = data.DateTime.ToString(CultureInfo.InvariantCulture);
      message.text = data.Text;
      
      read.SetActive(!data.Read);
    }
    
    public void OnBecomeVisible(ScrollerPanelSide side)
    {
      var message = Communicator.MessagesBank[_userName][_index];
      message.Read = true;
      Communicator.MessagesBank[_userName][_index] = message;
      
      read.SetActive(false);
      
      BlackBox.SaveMessages(Communicator.MessagesBank);
    }

    public void OnBecomeInvisible(ScrollerPanelSide side)
    {
    }
  }
}