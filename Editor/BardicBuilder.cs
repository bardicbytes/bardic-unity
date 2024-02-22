//alex@bardicbytes.com

using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;

namespace BardicBytes.BardicUnity.Editor
{ 
    [CreateAssetMenu(menuName = "BardicBytes/Builder")]
    public class BardicBuilder : ScriptableObject, IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        [SerializeField] private UnityEvent onEnvirontmentChange,onPreBuild, onPostBuild = default;

        [SerializeField] private bool devBuild,deepProfiling, scriptDebugging = false;
        
        [SerializeField] private SceneAsset[] scenes;

        [SerializeField] private BuildTarget target = BuildTarget.StandaloneWindows;
        [SerializeField] private BuildTargetGroup group = BuildTargetGroup.Standalone;

        [SerializeField] private string[] defines;
        [SerializeField] private string fileName = "fileName.exe";
        [SerializeField] private string path = "";

        string PathSuffix => target == BuildTarget.WebGL ? "" : string.Format(@"\{0}", fileName);
        public string[] EditorFieldNames => new string[] { "scenes" };
        public bool DrawOtherFields => true;
        public int callbackOrder { get { return 0; } }

        public void OnPostprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var len = Application.dataPath.Length - "Assets/".Length;
            var p = Application.dataPath.Substring(0, len);

            for (int i = 0; i < report.GetFiles().Length; i++)
            {
                sb.Append(report.GetFiles()[i].path.Replace(p, ""));
                if (i != report.GetFiles().Length - 1) sb.Append(", ");
            }
            UnityEngine.Debug.Log($"Build Complete. {report.GetFiles().Length} files. \n{sb}");
        }

        public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
        {

            var p = Path.Combine(Application.dataPath, "Data", "buildinfo.json");
            
            if (File.Exists(p))
            {
                string json = System.IO.File.ReadAllText(p);
                var bi = JsonUtility.FromJson<BuildInfo>(json);
                bi.Update();
                File.WriteAllText(p, bi.ToJson());
                UnityEngine.Debug.Log($"{bi}\nBardicBuildPreprocessor.OnPreprocessBuild for target {report.summary.platform} at path {report.summary.outputPath}");
            }
        }

        [ContextMenu("Set Environment")]
        public void SetEnvironment()
        {
            UnityEngine.Debug.Log("Setting Platform Environment " + name);
            onEnvirontmentChange.Invoke();
            if(defines != null && defines.Length > 0) PlayerSettings.SetScriptingDefineSymbolsForGroup(group, defines);
            EditorUserBuildSettings.SwitchActiveBuildTarget(group, target);
        }

        [ContextMenu("Start Build")]
        public void BuildGame()
        {
            UnityEngine.Debug.Assert(scenes != null && scenes.Length > 0, "Scenes array is empty.");

            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
                }

                EditorUtility.DisplayProgressBar("Building Game", "Setting Build Environment...", .25f);

                SetEnvironment();

                if (string.IsNullOrEmpty(path))
                {
                    UnityEngine.Debug.LogWarning("path is empty");
                    EditorUtility.ClearProgressBar();
                    return;
                }

                EditorUtility.DisplayProgressBar("Building Game", "Working...", .4f);
                string[] scenePaths = new string[scenes.Length];

                for (int i = 0; i < scenes.Length; i++)
                {
                    scenePaths[i] = AssetDatabase.GetAssetPath(scenes[i]);
                }

                EditorUtility.DisplayProgressBar("Building Game", "Building...", .5f);

                var report = BuildPipeline.BuildPlayer(scenePaths, path + PathSuffix, target, GetBuildOptions());

                EditorUtility.DisplayProgressBar("Building Game", "Wrapping Up...", .85f);

                if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                {
                    OpenFolder();
                }

                var bi = BuildInfo.LoadDefault();
                UnityEngine.Debug.Log("Built " + name + " " + bi + " " + " to " + path);
                onPostBuild.Invoke();
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            BuildOptions GetBuildOptions()
            {
                BuildOptions bo = BuildOptions.None;
                if (devBuild) bo |= BuildOptions.Development;
                if (devBuild && deepProfiling) bo |= BuildOptions.EnableDeepProfilingSupport;
                if (devBuild && scriptDebugging) bo |= BuildOptions.AllowDebugging;

                return bo;
            }
        }

        [ContextMenu("Open Folder")]
        private void OpenFolder()
        {
            if (Directory.Exists(path))
            {
                string p = path.Replace(@"/", @"\");
                var args = string.Format("{1}{0}{1}", p, "\"");
                //UnityEngine.Debug.Log("Opening Explorer: " + args);
                var si = new ProcessStartInfo("explorer.exe", args);
                Process.Start(si);
            }
        }
    }
}