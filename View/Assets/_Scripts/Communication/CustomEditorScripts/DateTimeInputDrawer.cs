using System;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Communication.CustomEditorScripts
{
  [CustomPropertyDrawer(typeof(DateTimeInputAttribute))]
  public class DateTimeInputDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginProperty(position, label, property);

      EditorGUI.BeginChangeCheck();

      // Get the DateTime value from the property
      int dateValue = property.FindPropertyRelative("date").intValue;
      int timeValue = property.FindPropertyRelative("time").intValue;
      DateTime dateTimeValue = DateTime.FromBinary((long)dateValue << 32 | timeValue);

      // Display a custom input field for date and time
      string newDateTimeString = EditorGUI.TextField(position, label, dateTimeValue.ToString());

      if (EditorGUI.EndChangeCheck())
      {
        DateTime newDateTimeValue;
        if (DateTime.TryParse(newDateTimeString, out newDateTimeValue))
        {
          // Update the property with the new DateTime value
          property.FindPropertyRelative("date").intValue = (int)(newDateTimeValue.Ticks >> 32);
          property.FindPropertyRelative("time").intValue = (int)(newDateTimeValue.Ticks & 0xFFFFFFFF);
        }
      }

      EditorGUI.EndProperty();
    }
  }
}