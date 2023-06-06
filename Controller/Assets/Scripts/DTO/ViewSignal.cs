using System;
using System.Collections.Generic;
using Enum;

namespace DTO
{
  [Serializable]
  public class ViewSignal
  {
    public ViewOperation Operation;
    
    public string UserName;

    public string Message;
    
    public DateTime DateTime;
    
    public string VideoId;
    
    public List<SubtitlePart> Subtitles;

    public ViewSignal(
      ViewOperation operation,
      string userName,
      string message = default,
      DateTime dateTime = default,
      string videoId = default,
      List<SubtitlePart> subtitles = default)
    {
      Operation = operation;
      UserName = userName;
      Message = message;
      DateTime = dateTime;
      VideoId = videoId;
      Subtitles = subtitles;
    }
  }
}