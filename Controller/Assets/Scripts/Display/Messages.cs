using System;
using System.Collections.Generic;
using Component;
using DTO;
using Enum;
using Newtonsoft.Json;
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

    private void ReceiveCall()
    {
      var signal = new ViewSignal(ViewOperation.Call, "Clement", default, DateTime.Now, "Clem_Call_Source", true, true,
        new List<SubtitlePart> { new("Hi", 1f, 2f) });

      var json = JsonConvert.SerializeObject(signal);

      HeadControl.Instance.Communicator.ProcessSignal(json);
    }

    private void ClearAllMessages()
    {
      BlackBox.ClearMessages();
      HeadControl.Instance.MessagesBank.Clear();
      list.Refresh();
      bubble.Refresh();
    }
  }
}