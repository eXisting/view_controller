using System;
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

    public ViewSignal(
      ViewOperation operation,
      string userName,
      string message,
      DateTime dateTime = default,
      string videoId = default)
    {
      Operation = operation;
      UserName = userName;
      Message = message;
      DateTime = dateTime;
      VideoId = videoId;
    }
  }
}