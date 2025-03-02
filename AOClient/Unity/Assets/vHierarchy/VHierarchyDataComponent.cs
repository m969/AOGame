#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using Type = System.Type;
using static VHierarchy.VHierarchyData;
using static VHierarchy.Libs.VUtils;
using static VHierarchy.Libs.VGUI;



namespace VHierarchy
{
    [ExecuteInEditMode]
    public abstract class VHierarchyDataComponent : MonoBehaviour, ISerializationCallbackReceiver
    {
        public void Awake()
        {
            void register()
            {
                VHierarchy.dataComponents_byScene[gameObject.scene] = this;
            }
            void handleSceneDuplication()
            {
                if (sceneData == null) return;
                if (!sceneData.goDatas_byGlobalId.Any()) return;


                var curSceneGuid = gameObject.scene.path.ToGuid();
                var dataSceneGuid = sceneData.goDatas_byGlobalId.Keys.First().guid;

                if (curSceneGuid == dataSceneGuid) return;


                var newDic = new SerializableDictionary<GlobalID, GameObjectData>();

                foreach (var kvp in sceneData.goDatas_byGlobalId)
                    newDic[new GlobalID(kvp.Key.ToString().Replace(dataSceneGuid, curSceneGuid))] = kvp.Value;


                sceneData.goDatas_byGlobalId = newDic;


                EditorSceneManager.MarkSceneDirty(gameObject.scene);
                EditorSceneManager.SaveScene(gameObject.scene);

            }

            register();
            handleSceneDuplication();

        }

        public SceneData sceneData;


        public void OnBeforeSerialize() => VHierarchy.firstDataCacheLayer.Clear();
        public void OnAfterDeserialize() => VHierarchy.firstDataCacheLayer.Clear();



        [CustomEditor(typeof(VHierarchyDataComponent), true)]
        class Editor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                var style = EditorStyles.label;
                style.wordWrap = true;


                SetGUIEnabled(false);
                BeginIndent(0);

                Space(4);
                EditorGUILayout.LabelField("This component stores vHierarchy icons and colors that are assigned to objects in this scene", style);

                Space(2);

                EndIndent(10);
                ResetGUIEnabled();

            }
        }

    }
}
#endif