using System;
using Enum;
using Newtonsoft.Json;
using UnityEngine;

namespace DTO
{
  [Serializable]
  public class ControllerSignal
  {
    public ControllerOperation Operation;
    
    // Ignore due to JsonSerializationException: Self referencing loop detected for property 'normalized' with type
    [JsonIgnore]
    public Vector2 Direction
    {
      set
      {
        x = value.x;
        y = value.y;
      }
    }
    
    public float x;
    public float y;

    public ControllerSignal(ControllerOperation operation, Vector2 direction = default)
    {
      Operation = operation;
      x = direction.x;
      y = direction.y;
      
      Direction = direction;
    }
  }
}