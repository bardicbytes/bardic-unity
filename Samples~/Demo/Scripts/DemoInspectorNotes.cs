using UnityEngine;

namespace BardicBytes.BardicUnity.Editor
{
    public class DemoInspectorNotes : MonoBehaviour
    {
#if UNITY_EDITOR
        [ReadOnly]
        [TextArea(5, 10)]
        [Tooltip("editor only component for samples")]
        public string notes;
#endif
    }
}