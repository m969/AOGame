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
using Type = System.Type;
using static VHierarchy.VHierarchyData;
using static VHierarchy.Libs.VUtils;
using static VHierarchy.Libs.VGUI;



namespace VHierarchy
{
    [FilePath("Library/vHierarchy Cache.asset", FilePathAttribute.Location.ProjectFolder)]
    public class VHierarchyCache : ScriptableSingleton<VHierarchyCache>
    {
        // used for finding SceneData and SceneIdMap for objects that were moved out of their original scene 
        public SerializableDictionary<int, string> originalSceneGuids_byInstanceId = new SerializableDictionary<int, string>();

        // used as cache for converting GlobalID to InstanceID and as a way to find GameObjectData for prefabs in playmode (when prefabs produce invalid GlobalIDs)
        public SerializableDictionary<string, SceneIdMap> sceneIdMaps_bySceneGuid = new SerializableDictionary<string, SceneIdMap>();


        [System.Serializable]
        public class SceneIdMap
        {
            public SerializableDictionary<int, GlobalID> globalIds_byInstanceId = new SerializableDictionary<int, GlobalID>();

            public int instanceIdsHash;
            public int globalIdsHash;

        }





        public static void Clear()
        {
            instance.originalSceneGuids_byInstanceId.Clear();
            instance.sceneIdMaps_bySceneGuid.Clear();

            instance.Save(true);

        }

        // public static void Save() => instance.Save(true);    // cache is never saved to disk, it just needs to survive domain reloads


    }
}
#endif