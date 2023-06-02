using System;
using System.Collections.Generic;
using System.IO;
using DTO;
using UnityEngine;

namespace Component
{
  public static class BlackBox
  {
    private static readonly string PathToFile = $@"{Application.persistentDataPath}/Messages";

    public static event Action NewData;
    
    public static string MessagesJson;

    static BlackBox()
    {
      if (!File.Exists(PathToFile))
        return;
      
      using var file = new StreamReader(PathToFile);
      MessagesJson = file.ReadToEnd();
    }

    public static void SaveMessages(Dictionary<string, List<MessageData>> messagesBank)
    {
      var json = Newtonsoft.Json.JsonConvert.SerializeObject(messagesBank);
      
      if (!File.Exists(PathToFile))
      {
        using var newFile = File.CreateText(PathToFile);
        newFile.Write(json);
        MessagesJson = json;
        NewData?.Invoke();
        return;
      }
      
      using var file = new StreamWriter(PathToFile);
      file.Write(json);
      MessagesJson = json;
      
      NewData?.Invoke();
    }

    public static void ClearMessages()
    {
      File.WriteAllText(PathToFile, string.Empty);
    }
  }
}