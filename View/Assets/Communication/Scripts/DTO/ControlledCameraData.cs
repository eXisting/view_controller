namespace Communication.Scripts.DTO
{
  public struct ControlledCameraData
  {
    public readonly float speed;

    public bool freeCamera;
    public bool blockHorizontal;
    public bool blockVertical;
    
    public readonly float left;
    public readonly float right;
    public readonly float up;
    public readonly float down;

    public ControlledCameraData(float speed,
      bool freeCamera,
      bool blockHorizontal,
      bool blockVertical,
      float left,
      float right,
      float up,
      float down)
    {
      this.speed = speed;

      this.freeCamera = freeCamera;
      this.blockHorizontal = blockHorizontal;
      this.blockVertical = blockVertical;
      
      this.left = left;
      this.right = right;
      this.up = up;
      this.down = down;
    }
  }
}