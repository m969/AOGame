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
        public static T Load<T>(string path)
        {
            return default(T);
        }
#endif
    }
}