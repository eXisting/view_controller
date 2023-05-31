namespace _Scripts.Communication.TDO
{
  public struct RotationRestrictions
  {
    public readonly float left;
    public readonly float right;
    public readonly float up;
    public readonly float down;

    public RotationRestrictions(float left, float right, float up, float down)
    {
      this.left = left;
      this.right = right;
      this.up = up;
      this.down = down;
    }
  }
}