//alex@bardicbytes.com

using UnityEditor;
using UnityEngine;

namespace BardicBytes.BardicUnity.Editor
{
    [CustomEditor(typeof(BardicBuilder))]
    public class BardicBuilderEditor : UnityEditor.Editor
    {
        public BardicBuilder Target => target as BardicBuilder;
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Build Now")) Target.BuildGame();
            GUILayout.Space(10);
            if (GUILayout.Button("Set Environment")) Target.SetEnvironment();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);


            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}