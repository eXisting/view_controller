using System;
using Enum;
using UnityEngine;

namespace Tdo
{
  [Serializable]
  public class ViewSignal
  {
    public ViewOperation Operation;
    
    public Vector2 Direction;

    public ViewSignal(ViewOperation operation, Vector2 direction = default)
    {
      Operation = operation;
      Direction = direction;
    }
  }
}