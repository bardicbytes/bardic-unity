//alex@bardicbytes.com

using UnityEditor;
using UnityEngine;

namespace BardicBytes.BardicUnity.Editor
{
    [CustomEditor(typeof(UpdateAggregator))]
    public class UpdateAggregatorEditor : UnityEditor.Editor
    {
        public UpdateAggregator Target => target as UpdateAggregator;
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Populate with Self")) Target.RefreshReceiversSelf();
            GUILayout.Space(10);
            if (GUILayout.Button("Populate with Scene")) Target.RefreshReceiversScene();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}