using Communication.Scripts.Controls;
using UnityEditor;

namespace Communication.Scripts.Editor
{
  [CustomEditor(typeof(ControlledCamera)), CanEditMultipleObjects]
  public class ControlledCameraEditor : UnityEditor.Editor
  {
    private SerializedProperty speed;

    private SerializedProperty freeCamera;
    private SerializedProperty  blockHorizontal;
    private SerializedProperty  blockVertical;
    
    private SerializedProperty  leftLimit;
    private SerializedProperty  rightLimit;
    private SerializedProperty  topLimit;
    private SerializedProperty  bottomLimit;

    private void OnEnable()
    {
      speed = serializedObject.FindProperty("speed");
      freeCamera = serializedObject.FindProperty("freeCamera");
      blockHorizontal = serializedObject.FindProperty("blockHorizontal");
      blockVertical = serializedObject.FindProperty("blockVertical");
      leftLimit = serializedObject.FindProperty("leftLimit");
      rightLimit = serializedObject.FindProperty("rightLimit");
      topLimit = serializedObject.FindProperty("topLimit");
      bottomLimit = serializedObject.FindProperty("bottomLimit");
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      EditorGUILayout.PropertyField(speed);

      EditorGUILayout.PropertyField(blockHorizontal);
      EditorGUILayout.PropertyField(blockVertical);
      
      if (!(blockHorizontal.boolValue && blockVertical.boolValue))
        EditorGUILayout.PropertyField(freeCamera);

      if (!freeCamera.boolValue && !blockHorizontal.boolValue && !blockVertical.boolValue)
      {
        EditorGUILayout.PropertyField(leftLimit);
        EditorGUILayout.PropertyField(rightLimit);
        EditorGUILayout.PropertyField(topLimit);
        EditorGUILayout.PropertyField(bottomLimit);
        
        serializedObject.ApplyModifiedProperties();
        return;
      }
      
      if (!blockHorizontal.boolValue && !freeCamera.boolValue)
      {
        EditorGUILayout.PropertyField(leftLimit);
        EditorGUILayout.PropertyField(rightLimit);
      }
      
      if (!blockVertical.boolValue && !freeCamera.boolValue)
      {
        EditorGUILayout.PropertyField(topLimit);
        EditorGUILayout.PropertyField(bottomLimit);
      }

      serializedObject.ApplyModifiedProperties();
    }
  }
}
