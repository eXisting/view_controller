using System;
using System.Collections.Generic;
using Communication.Scripts.Enum;
using UnityEngine;
using UnityEngine.Serialization;

namespace Communication.Scripts.DTO
{
  [CreateAssetMenu(fileName = "Signal", menuName = "ScriptableObjects/Signal", order = 1)]
  public class ViewSignalScriptable : ScriptableObject
  {
    [FormerlySerializedAs("Operation")] public ViewOperation operation;
    
    [FormerlySerializedAs("UserName")] public string userName;

    [FormerlySerializedAs("Message")] public string message;
    
    public DateTime Time;
    
    [FormerlySerializedAs("VideoId")] public string videoId;
    
    public List<SubtitlePart> subtitles;
  }
}