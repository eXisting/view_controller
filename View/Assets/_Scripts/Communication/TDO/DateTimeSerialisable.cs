using System;

namespace _Scripts.Communication.TDO
{
  [Serializable]
  public struct DateTimeSerializable
  {
    public int date;
    public int time;

    public DateTime ToDateTime()
    {
      var ticks = ((long)date << 32) | (uint)time;
      return DateTime.FromBinary(ticks);
    }

    public static DateTimeSerializable FromDateTime(DateTime dateTime)
    {
      DateTimeSerializable dateTimeSerializable;
      dateTimeSerializable.date = (int)(dateTime.Ticks >> 32);
      dateTimeSerializable.time = (int)(dateTime.Ticks & 0xFFFFFFFF);
      
      return dateTimeSerializable;
    }
  }
}