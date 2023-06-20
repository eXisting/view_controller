using System.Globalization;
using Component;
using Component.Communicators;
using DTO;
using TMPro;
using UnityEngine;
using UnlimitedScrollUI;

namespace UI.Messages
{
  public class MessageCard : MonoBehaviour
  {
    [SerializeField] private TMP_Text date;
    [SerializeField] private TMP_Text message;

    [SerializeField] private GameObject read;

    private string _userName;
    private int _index;
    
    public void OnEnable()
    {
      var messageData = HeadControl.Instance.MessagesBank[_userName][_index];
      messageData.Read = true;
      HeadControl.Instance.MessagesBank[_userName][_index] = messageData;
      
      read.SetActive(false);
      
      BlackBox.SaveMessages(HeadControl.Instance.MessagesBank);
    }
    
    public void Setup(MessageData data, int index)
    {
      _userName = data.UserName;
      _index = index;
      
      date.text = data.DateTime.ToString(CultureInfo.InvariantCulture);
      message.text = data.Text;
      
      read.SetActive(!data.Read);
    }
  }
}