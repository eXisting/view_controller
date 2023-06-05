using System;
using Communication.Scripts.Enum;
using UnityEngine;

namespace Communication.Scripts.TDO
{
  [Serializable]
  public class ControllerSignal
  {
    public ControllerOperation Operation;
    
    public Vector2 Direction;

    public ControllerSignal(ControllerOperation operation, Vector2 direction = default)
    {
      Operation = operation;
      Direction = direction;
    }
  }
}