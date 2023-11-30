namespace AO
{
    using AO;
    using ET;
    using ET.Server;
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 网络服务群组的本地缓存
    /// </summary>
    public class NetWorld
    {
        public GlobalAppCall GlobalAppCall {get;set;}

        //public static Dictionary<Type, List<long>> AppIds = new();
        //public static Dictionary<long, AppConfig> AppConfigs = new();
        //public static Dictionary<int, AppConfig> ZoneConfigs = new();


        //public static void AppRegister(AppConfig appConfig, Entity app)
        //{
        //    AppConfigs.Add(appConfig.Id, appConfig);
        //    AppIds.Add(app.GetType(), new List<long> { app.InstanceId });
        //}

        //public static AppConfig GetAppConfig(long id)
        //{
        //    return AppConfigs[id];
        //}

        //public static long GetAppId<T>(int index = 0) where T : IApp
        //{
        //    return AppIds[typeof(T)][index];
        //}

        //public static T GetAppCall<T>(int index = 0) where T : Entity, IAwake<long>, new()
        //{
        //    var appTypeName = typeof(T).FullName.TrimEnd("Call".ToCharArray());
        //    var appType = Type.GetType(appTypeName);
        //    var appId = AppIds[appType][index];
        //    return GetEntityCall<T>(appId);
        //}

        //public static T GetEntityCall<T>(long entityActorId) where T : Entity, IAwake<long>, new()
        //{
        //    var entityCall = AOGame.Root.GetChild<T>(entityActorId);
        //    if (entityCall != null)
        //    {
        //        return entityCall;
        //    }
        //    entityCall = AOGame.Root.AddChildWithId<T, long>(entityActorId, entityActorId);
        //    return entityCall;
        //}
    }
}