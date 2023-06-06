namespace DTO
{
  public struct SubtitlePart
  {
    public string Text;
    public float Start;
    public float Finish;

    public SubtitlePart(string text, float start, float finish)
    {
      Text = text;
      Start = start;
      Finish = finish;
    }
  }
}