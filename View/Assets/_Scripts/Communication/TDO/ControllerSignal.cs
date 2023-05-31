using System;
using _Scripts.Communication.Enum;
using UnityEngine;

namespace _Scripts.Communication.TDO
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