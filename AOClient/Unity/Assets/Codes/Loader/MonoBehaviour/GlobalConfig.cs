using UnityEngine;

namespace ET
{
    public enum CodeMode
    {
        Client = 1,
        Server = 2,
        ClientServer = 3,
    }
    
    [CreateAssetMenu(menuName = "ET/CreateGlobalConfig", fileName = "GlobalConfig", order = 0)]
    public class GlobalConfig: ScriptableObject
    {
        public CodeMode CodeMode;
#if UNITY_EDITOR
        public UnityEditorInternal.AssemblyDefinitionAsset[] AssemblyDefinitions;
#endif
    }
}