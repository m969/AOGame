namespace AO
{
    using AO;
    using ET;
    using ET.Server;
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class AOGlobal
    {
        public static Dictionary<Type, List<long>> AppIds = new();
        public static Dictionary<long, AppConfig> AppConfigs = new();
        public static Dictionary<int, AppConfig> ZoneConfigs = new();


        public static void AppRegister(AppConfig appConfig, Entity app)
        {
            AppConfigs.Add(appConfig.Id, appConfig);
            AppIds.Add(app.GetType(), new List<long> { app.InstanceId });
        }

        public static AppConfig GetAppConfig(long id)
        {
            return AppConfigs[id];
        }

        public static long GetAppId<T>(int index = 0) where T : IApp
        {
            return AppIds[typeof(T)][index];
        }

        public static T GetAppCall<T>(int index = 0) where T : Entity, IAwake<long>, new()
        {
            var appTypeName = typeof(T).FullName.TrimEnd("Call".ToCharArray());
            var appType = Type.GetType(appTypeName);
            var appId = AppIds[appType][index];
            var appCall = AOGame.Root.GetChild<T>(appId);
            if (appCall != null)
            {
                return appCall;
            }
            appCall = AOGame.Root.AddChildWithId<T, long>(appId, appId);
            return appCall;
        }
    }
}