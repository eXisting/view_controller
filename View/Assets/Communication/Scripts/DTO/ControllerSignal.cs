using Communication.Scripts.Enum;
using Newtonsoft.Json;
using UnityEngine;

namespace Communication.Scripts.DTO
{
  [JsonObject(MemberSerialization.OptOut)]
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