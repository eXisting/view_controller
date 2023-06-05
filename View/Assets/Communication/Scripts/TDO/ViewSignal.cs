using System;
using Communication.Scripts.Enum;

namespace Communication.Scripts.TDO
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