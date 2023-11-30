namespace AO
{
    using AO;
    using ET;
    using ET.Server;
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    public static class AOGame
    {
        public static NetWorld NetWorld;
        public static Root Root;
        public static Entity DomainApp;
        public static bool IsDistribution;

        public static ActorIdApp ActorIdApp;
        public static DBConnectApp DBConnectApp;
        public static DBCacheApp DBCacheApp;
        public static RealmApp RealmApp;
        public static GateApp GateApp;
        public static MapApp MapApp;
        public static WorldServiceApp WorldServiceApp;

        public static Dictionary<Type, List<long>> AppIds = new();
        public static Dictionary<long, AppConfig> AppConfigs = new();
        public static Dictionary<int, AppConfig> ZoneConfigs = new();


        public static void Start(Root root, string appType)
        {
            Root = root;
            Time.GameTime = TimeHelper.ServerNow();
            root.AddComponent<AppTypeComponent, string>(appType);
        }

        public static void Run()
        {
            Time.DeltaTime = TimeHelper.ServerNow() - Time.GameTime;
            Time.deltaTime = Time.DeltaTime / 1000f;
            Time.GameTime = TimeHelper.ServerNow();
        }

        public static void InstallApp(AppConfig appConfig)
        {
            var instanceId = new InstanceIdStruct(Options.Instance.Process, (uint)appConfig.Id).ToLong();
            var app = Root.AddChildWithId(Type.GetType($"AO.{appConfig.Type}"), appConfig.Id, instanceId);
            (app as IApp).Zone = appConfig.Zone;

            AOZone.AppRegister(appConfig, app);
            app.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);
            if (app is ActorIdApp) ActorIdApp = (ActorIdApp)app;
            if (app is DBCacheApp) DBCacheApp = (DBCacheApp)app;
            if (app is DBConnectApp) DBConnectApp = (DBConnectApp)app;
            if (app is RealmApp) RealmApp = (RealmApp)app;
            if (app is GateApp) GateApp = (GateApp)app;
            if (app is MapApp) MapApp = (MapApp)app;
            if (app is WorldServiceApp) WorldServiceApp = (WorldServiceApp)app;

            if (app is MapApp) DomainApp = MapApp;
        }

        public static async ETTask PublishAsync<T>(T a) where T : struct
        {
            await EventSystem.Instance.PublishAsync(Root, a);
        }

        public static void Publish<T>(T a) where T : struct
        {
            EventSystem.Instance.Publish(Root, a);
        }

        public static void PublishServer<T>(T a) where T : struct
        {
            EventSystem.Instance.Publish(Root, a);
        }
    }
}