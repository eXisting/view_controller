using System;
using System.Collections.Generic;
using Communication.Scripts.Enum;
using UnityEngine;
using UnityEngine.Serialization;

namespace Communication.Scripts.DTO
{
  [CreateAssetMenu(fileName = "Signal", menuName = "Communication/Signal", order = 1)]
  public class ViewSignalScriptable : ScriptableObject
  {
    public ViewOperation operation;
    
    public string userName;

    public string message;

    public string videoId;
    
    public bool loopVideo;

    public bool muteVideo;
    
    public List<SubtitlePart> subtitles;
  }
}