using System;
using Enum;

namespace Tdo
{
  [Serializable]
  public class ControllerSignal
  {
    public ControllerOperation Operation;
    
    public string UserId;

    public string Message;
    
    public DateTime Time;
    
    public string VideoId;

    public ControllerSignal(
      ControllerOperation operation,
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