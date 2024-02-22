//alex@bardicbytes.com

using UnityEngine;

namespace BardicBytes.BardicUnity.EditorSamples
{
    [CreateAssetMenu(menuName = "BardicBytes/Demo/DemoAsset")]
    public class DemoAsset : ScriptableObject
    {
        [field: SerializeField]
        public string DemoText { get; private set; }

        public void PrintDemoText() => Debug.Log($"{name}: DemoText={DemoText}");

        public void Log(string s) => Debug.Log(s);
    }
}