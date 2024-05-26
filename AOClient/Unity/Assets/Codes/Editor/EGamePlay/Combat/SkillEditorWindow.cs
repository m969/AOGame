//using Sirenix.OdinInspector.Editor;
//using Sirenix.Utilities;
//using Sirenix.Utilities.Editor;
//using UnityEditor;
//using UnityEngine;
//using System.Linq;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using Object = UnityEngine.Object;
//using ET;
//using EGamePlay.Combat;

//public class SkillEditorWindow : OdinMenuEditorWindow
//{
//    public static string SkillConfigObjectsPath = "Assets/EGPsExamples/RpgExample/Resources/SkillConfigs";
//    public static string ExecutionObjectsPath = "Assets/EGPsExamples/RpgExample/Resources";

//    public Dictionary<string, ExecutionObject> ExecutionObjects = new Dictionary<string, ExecutionObject>();


//    public class SkillConfigData
//    {
//        public SkillConfigObject ConfigObject;
//        public ExecutionObject ExecutionObject;
//    }

//    [MenuItem("Tools/EGamePlay/SkillEditorWindow")]
//    private static void Open()
//    {
//        var window = GetWindow<SkillEditorWindow>();
//        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1200, 800);
//    }

//    SkillConfigCategory SkillConfigCategory;

//    int totalCount = 0;
//    protected override OdinMenuTree BuildMenuTree()
//    {
//        var tree = new OdinMenuTree(true);
//        tree.DefaultMenuStyle.IconSize = 32.00f;
//        tree.Config.DrawSearchToolbar = true;

//        var configsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Bundles/Configs.prefab");
//        var assembly = System.Reflection.Assembly.GetAssembly(typeof(TimerManager));
//        var configsCollector = configsPrefab.GetComponent<ReferenceCollector>();
//        if (configsCollector != null)
//        {
//            var configText = configsCollector.Get<TextAsset>("SkillConfig");
//            var configTypeName = $"ET.SkillConfig";
//            var configType = assembly.GetType(configTypeName);
//            var typeName = $"ET.SkillConfigCategory";
//            var configCategoryType = assembly.GetType(typeName);
//            var configCategory = Activator.CreateInstance(configCategoryType) as ET.SkillConfigCategory;
//            configCategory.ConfigText = configText.text;
//            configCategory.BeginInit();
//            SkillConfigCategory = configCategory;
//        }
//        var allSkill = SkillConfigCategory.GetAll();
//        foreach (var item in allSkill.Values)
//        {
//            var path = $"{SkillConfigObjectsPath}/Skill_{item.Id}.asset";
//            var path2 = $"{ExecutionObjectsPath}/Execution_{item.Id}.asset";
//            var asset = AssetDatabase.LoadAssetAtPath<SkillConfigObject>(path);
//            var asset2 = AssetDatabase.LoadAssetAtPath<ExecutionObject>(path2);
//            //var data = new SkillConfigData();
//            //data.ConfigObject = asset;
//            //data.ExecutionObject = asset2;
//            var key = $"{item.Id}_{item.Name}";
//            tree.Add(key, asset);
//            ExecutionObjects[key] = asset2;
//            //tree.AddAllAssetsAtPath("", "Assets/EGPsExamples/Resources/SkillConfigs/", typeof(SkillConfigObject), true);
//        }
//        //tree.AddAllAssetsAtPath("", "Assets/Plugins/Sirenix/Demos/SAMPLE - RPG Editor/Items", typeof(Item), true);
//        //.ForEach(this.AddDragHandles);

//        //tree.EnumerateTree().AddIcons<Item>(x => x.Icon);

//        return tree;
//    }

//    protected override void OnBeginDrawEditors()
//    {
//        var selected = this.MenuTree.Selection.FirstOrDefault();
//        var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

//        bool changed = false;
//        SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
//        {
//            var data = selected?.Value as SkillConfigObject;
//            EditorGUILayout.ObjectField(data, typeof(SkillConfigObject), false);
//            if (GUILayout.Button("Select In Editor"))
//            {
//                Selection.objects = this.MenuTree.Selection.Where(_ => _.Value is SkillConfigObject)
//                    .Select(_ => (_.Value as SkillConfigObject)).ToArray();
//            }
//        }
//        SirenixEditorGUI.EndHorizontalToolbar();

//        SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
//        {
//            var data = ExecutionObjects[selected?.Name];
//            if (data != null)
//            {
//                EditorGUILayout.ObjectField(data, typeof(ExecutionObject), false);
//                if (GUILayout.Button("Select In Editor"))
//                {
//                    EditorGUIUtility.PingObject(data);
//                }
//            }
//        }
//        SirenixEditorGUI.EndHorizontalToolbar();

//        if (changed)
//        {
//            ForceMenuTreeRebuild();
//        }
//    }
//}
