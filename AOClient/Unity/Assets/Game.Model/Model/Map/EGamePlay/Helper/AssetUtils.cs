using EGamePlay.Combat;
using ET;
using System.IO;

namespace GameUtils
{
    public static class AssetUtils
    {
#if UNITY
        public static T Load<T>(string path) where T : UnityEngine.Object
        {
            return UnityEngine.Resources.Load<T>(path);
        }
#else
        public static T Load<T>(string name)
        {
            var text = File.ReadAllText($"../../SkillConfigs/{name}.json");
            var obj = JsonHelper.FromJson<T>(text);
            return obj;
        }
#endif
    }
}