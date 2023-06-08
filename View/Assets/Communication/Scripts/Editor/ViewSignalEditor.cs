using Communication.Scripts.DTO;
using Communication.Scripts.Enum;
using UnityEditor;

namespace Communication.Scripts.Editor
{
    [CustomEditor(typeof(ViewSignalScriptable))]
    public class ViewSignalScriptableEditor : UnityEditor.Editor
    {
        private SerializedProperty operation;
        private SerializedProperty userName;
        private SerializedProperty message;
        private SerializedProperty videoId;
        private SerializedProperty loopVideo;
        private SerializedProperty muteVideo;
        private SerializedProperty subtitles;

        private void OnEnable()
        {
            operation = serializedObject.FindProperty("operation");
            userName = serializedObject.FindProperty("userName");
            message = serializedObject.FindProperty("message");
            videoId = serializedObject.FindProperty("videoId");
            loopVideo = serializedObject.FindProperty("loopVideo");
            muteVideo = serializedObject.FindProperty("muteVideo");
            subtitles = serializedObject.FindProperty("subtitles");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(operation);

            ViewOperation selectedOperation = (ViewOperation)operation.enumValueIndex;

            if (selectedOperation == ViewOperation.Call)
            {
                EditorGUILayout.PropertyField(videoId);
                EditorGUILayout.PropertyField(loopVideo);
                EditorGUILayout.PropertyField(muteVideo);
                EditorGUILayout.PropertyField(subtitles);
            }
            else if (selectedOperation == ViewOperation.Message)
            {
                EditorGUILayout.PropertyField(userName);
                EditorGUILayout.PropertyField(message);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

