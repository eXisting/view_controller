using System;
using System.Collections.Generic;
using Component;
using Component.Communicators;
using DTO;
using Enum;
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

    private void OnEnable()
    {
      // var json = Newtonsoft.Json.JsonConvert.SerializeObject(new ViewSignal(operation: ViewOperation.Call, userName: "Andrew",
      //   videoId: "Clem_CallSource", subtitles: new List<SubtitlePart> { new("Hi 1-2", 1, 2), new("bye 5-6", 5, 6) }));
      //
      // HeadControl.Instance.Communicator.ProcessSignal(json);
    }

    private void ClearAllMessages()
    {
      BlackBox.ClearMessages();
      HeadControl.Instance.Communicator.MessagesBank.Clear();
      list.Refresh();
      bubble.Refresh();
    }
  }
}