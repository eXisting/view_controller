using System;

namespace Communication.Scripts.DTO
{
  [Serializable]
  public struct SubtitlePart
  {
    public string Text;
    public float Start;
    public float Finish;
  }
}