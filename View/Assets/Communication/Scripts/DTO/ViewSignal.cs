using System;
using System.Collections.Generic;
using Communication.Scripts.Enum;
using Newtonsoft.Json;

namespace Communication.Scripts.DTO
{
  [Serializable]
  public class ViewSignal
  {
    public ViewOperation Operation;

    public string UserName;

    public string Message;

    public DateTime DateTime;

    public string VideoId;

    public bool LoopVideo;

    public bool MuteVideo;

    public List<SubtitlePart> Subtitles;

    public ViewSignal(
      ViewOperation operation,
      string userName,
      string message = default,
      DateTime dateTime = default,
      string videoId = default,
      bool loopVideo = default,
      bool muteVideo = default,
      List<SubtitlePart> subtitles = default)
    {
      Operation = operation;
      UserName = userName;
      Message = message;
      DateTime = dateTime;
      VideoId = videoId;
      LoopVideo = loopVideo;
      MuteVideo = muteVideo;
      Subtitles = subtitles;
    }

    // if (subtitlesText == default) 
    //   return;
    //
    // Subtitles = new List<SubtitlePart>();
    //
    // foreach (var subtitleText in subtitlesText)
    // {
    //   SubtitlePart subtitles;
    //     
    //   var indexStart = subtitleText.LastIndexOf('{');
    //   var indexEnd = subtitleText.LastIndexOf('}');
    //
    //   var instructions = subtitleText.Substring(++indexStart, indexEnd - indexStart);
    //   var startFinish = instructions.Split('-');
    //   subtitles.Start = float.Parse(startFinish[0]);
    //   subtitles.Finish = float.Parse(startFinish[1]);
    //   subtitles.Text = subtitleText.Remove(indexStart - 2).TrimEnd();
    //   
    //   Subtitles.Add(subtitles);
    // }
  }
}