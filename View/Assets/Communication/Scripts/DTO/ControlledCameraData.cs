namespace Communication.Scripts.TDO
{
  public struct ControlledCameraData
  {
    public readonly float speed;
    
    public readonly float left;
    public readonly float right;
    public readonly float up;
    public readonly float down;

    public ControlledCameraData(float speed, float left, float right, float up, float down)
    {
      this.speed = speed;
      
      this.left = left;
      this.right = right;
      this.up = up;
      this.down = down;
    }
  }
}