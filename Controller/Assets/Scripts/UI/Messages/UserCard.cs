using System;
using System.Globalization;
using DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

namespace UI.Messages
{
  public class UserCard : MonoBehaviour, ICell
  {
    public event Action<string> Pressed;

    [SerializeField] private TMP_Text senderName;
    [SerializeField] private TMP_Text date;
    [SerializeField] private TMP_Text message;
    
    [SerializeField] private GameObject read;

    [SerializeField] private Button button;

    public void Setup(MessageData data, bool read)
    {
      senderName.text = data.UserName;
      date.text = data.DateTime.ToString(CultureInfo.InvariantCulture);
      message.text = data.Text;
      this.read.SetActive(!read);
      
      button.onClick.AddListener(NotifyPressed);
    }

    private void NotifyPressed()
    {
      Pressed?.Invoke(senderName.text);
    }


    public void OnBecomeVisible(ScrollerPanelSide side)
    {
    }

    public void OnBecomeInvisible(ScrollerPanelSide side)
    {
    }
  }
}