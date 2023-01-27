using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ET
{
    public static class BuildHelper
    {
        private const string relativeDirPrefix = "../Release";

        public static string BuildFolder = "../Release/{0}/StreamingAssets/";


        [MenuItem("AOGame/dll To .bytes ")]
        public static void DllToBytes()
        {
            //var selectTarget = Selection.activeObject;
            //var folderPath = AssetDatabase.GetAssetPath(selectTarget);
            var folderPath = "Assets/Bundles/AOTDllCode";
            var assetPaths = AssetDatabase.FindAssets("", new string[] { folderPath }).Select(x => AssetDatabase.GUIDToAssetPath(x)).Where(x => x.EndsWith(".dll"));
            var projectPath = Application.dataPath.TrimEnd("/Assets".ToCharArray());
            //var projectPath = Application.dataPath.Replace("/Assets", "");
            foreach (var item in assetPaths)
            {
                var filePath = projectPath + "/" + item;
                File.Copy(filePath, filePath + ".bytes", true);
                File.Delete(filePath);
            }
            AssetDatabase.Refresh();
        }

        //[InitializeOnLoadMethod]
        public static void ReGenerateProjectFiles()
        {
            if (Unity.CodeEditor.CodeEditor.CurrentEditor.GetType().Name== "RiderScriptEditor")
            {
                FieldInfo generator = Unity.CodeEditor.CodeEditor.CurrentEditor.GetType().GetField("m_ProjectGeneration", BindingFlags.Static | BindingFlags.NonPublic);
                var syncMethod = generator.FieldType.GetMethod("Sync");
                syncMethod.Invoke(generator.GetValue(Unity.CodeEditor.CodeEditor.CurrentEditor), null);
            }
            else
            {
                Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();
            }
            
            Debug.Log("ReGenerateProjectFiles finished.");
        }

        public const string DefineName = "ENABLE_DLL_LOAD";
#if ENABLE_DLL_LOAD
        [MenuItem("ET/Remove " + DefineName)]
        public static void RemoveEnableDllLoad()
        {
            EnableDllLoad(false);
        }
#else
        [MenuItem("ET/Add " + DefineName)]
        public static void AddEnableCodes()
        {
            EnableDllLoad(true);
        }
#endif
        private static void EnableDllLoad(bool enable)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var ss = defines.Split(';').ToList();
            if (enable)
            {
                if (ss.Contains(DefineName))
                {
                    return;
                }
                ss.Add(DefineName);
            }
            else
            {
                if (!ss.Contains(DefineName))
                {
                    return;
                }
                ss.Remove(DefineName);
            }
            defines = string.Join(";", ss);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
            AssetDatabase.SaveAssets();

            var assemblyDefinitions = Resources.Load<GlobalConfig>("GlobalConfig").AssemblyDefinitions;
            foreach (var assemblyDefinition in assemblyDefinitions)
            {
                var imp = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GetAssetPath(assemblyDefinition)).text;
                var path = AssetDatabase.GetAssetPath(assemblyDefinition);
                if (enable) imp = imp.Replace("\"Editor\"", "\"PS5\"");
                else imp = imp.Replace("\"PS5\"", "\"Editor\"");
                path = Application.dataPath.TrimEnd("Assets".ToCharArray()) + path;
                File.WriteAllText(path, imp);
            }
            AssetDatabase.Refresh();
        }

        //#if ENABLE_VIEW
        //        [MenuItem("ET/ChangeDefine/Remove ENABLE_VIEW")]
        //        public static void RemoveEnableView()
        //        {
        //            EnableView(false);
        //        }
        //#else
        //        [MenuItem("ET/ChangeDefine/Add ENABLE_VIEW")]
        //        public static void AddEnableView()
        //        {
        //            EnableView(true);
        //        }
        //#endif
        //        private static void EnableView(bool enable)
        //        {
        //            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        //            var ss = defines.Split(';').ToList();
        //            if (enable)
        //            {
        //                if (ss.Contains("ENABLE_VIEW"))
        //                {
        //                    return;
        //                }
        //                ss.Add("ENABLE_VIEW");
        //            }
        //            else
        //            {
        //                if (!ss.Contains("ENABLE_VIEW"))
        //                {
        //                    return;
        //                }
        //                ss.Remove("ENABLE_VIEW");
        //            }

        //            defines = string.Join(";", ss);
        //            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
        //            AssetDatabase.SaveAssets();
        //        }

        public static void Build(PlatformType type, BuildAssetBundleOptions buildAssetBundleOptions, BuildOptions buildOptions, bool isBuildExe, bool isContainAB, bool clearFolder)
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;
            string programName = "ET";
            string exeName = programName;
            switch (type)
            {
                case PlatformType.Windows:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    exeName += ".exe";
                    break;
                case PlatformType.Android:
                    buildTarget = BuildTarget.Android;
                    exeName += ".apk";
                    break;
                case PlatformType.IOS:
                    buildTarget = BuildTarget.iOS;
                    break;
                case PlatformType.MacOS:
                    buildTarget = BuildTarget.StandaloneOSX;
                    break;
                
                case PlatformType.Linux:
                    buildTarget = BuildTarget.StandaloneLinux64;
                    break;
            }

            string fold = string.Format(BuildFolder, type);

            if (clearFolder && Directory.Exists(fold))
            {
                Directory.Delete(fold, true);
            }
            Directory.CreateDirectory(fold);

            UnityEngine.Debug.Log("start build assetbundle");
            BuildPipeline.BuildAssetBundles(fold, buildAssetBundleOptions, buildTarget);

            UnityEngine.Debug.Log("finish build assetbundle");

            if (isContainAB)
            {
                FileHelper.CleanDirectory("Assets/StreamingAssets/");
                FileHelper.CopyDirectory(fold, "Assets/StreamingAssets/");
            }

            if (isBuildExe)
            {
                AssetDatabase.Refresh();
                string[] levels = {
                    "Assets/Scenes/Init.unity",
                };
                UnityEngine.Debug.Log("start build exe");
                BuildPipeline.BuildPlayer(levels, $"{relativeDirPrefix}/{exeName}", buildTarget, buildOptions);
                UnityEngine.Debug.Log("finish build exe");
            }
            else
            {
                if (isContainAB && type == PlatformType.Windows)
                {
                    string targetPath = Path.Combine(relativeDirPrefix, $"{programName}_Data/StreamingAssets/");
                    FileHelper.CleanDirectory(targetPath);
                    Debug.Log($"src dir: {fold}    target: {targetPath}");
                    FileHelper.CopyDirectory(fold, targetPath);
                }
            }
        }
    }
}
