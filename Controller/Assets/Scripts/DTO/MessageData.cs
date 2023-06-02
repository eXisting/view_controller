using System;

namespace DTO
{
  public struct MessageData
  {
    public string UserName;
    public string Text;
    public DateTime DateTime;

    public bool Read;

    public MessageData(string userName, string text, DateTime dateTime, bool read = false)
    {
      UserName = userName;
      Text = text;
      DateTime = dateTime;
      Read = read;
    }
  }
}