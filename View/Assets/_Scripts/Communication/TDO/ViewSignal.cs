using System;
using _Scripts.Communication.Enum;
using UnityEngine.Serialization;

namespace _Scripts.Communication.TDO
{
  [Serializable]
  public class ViewSignal
  {
    public ViewOperation Operation;
    
    public string UserName;

    public string Message;
    
    public DateTime Time;
    
    public string VideoId;

    public ViewSignal(
      ViewOperation operation,
      string userName,
      string message,
      DateTime time = default,
      string videoId = default)
    {
      Operation = operation;
      UserName = userName;
      Message = message;
      Time = time;
      VideoId = videoId;
    }
  }
}