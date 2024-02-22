//alex@bardicbytes.com

using UnityEditor;
using UnityEngine;

namespace BardicBytes.BardicUnity.Editor
{
    public class AssetUtility : EditorWindow
    {
        public static bool AddSubassetToScriptableObject(ScriptableObject parentObject, UnityEngine.Object subasset)
        {
            if(parentObject == null || subasset == null) return false;

            AssetDatabase.AddObjectToAsset(subasset, parentObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(parentObject);
            return true;
        }
    }
}