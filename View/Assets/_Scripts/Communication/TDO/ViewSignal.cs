using System;
using _Scripts.Communication.Enum;

namespace _Scripts.Communication.TDO
{
  [Serializable]
  public class ViewSignal
  {
    public ViewOperation Operation;
    
    public string UserId;

    public string Message;
    
    public DateTime Time;
    
    public string VideoId;

    public ViewSignal(
      ViewOperation operation,
      string userId,
      string message,
      DateTime time = default,
      string videoId = default)
    {
      Operation = operation;
      UserId = userId;
      Message = message;
      Time = time;
      VideoId = videoId;
    }
  }
}