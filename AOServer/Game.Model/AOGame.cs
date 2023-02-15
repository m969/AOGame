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
        public static Root Root;
        public static Root RootScene;
        public static Entity DomainApp;
        public static ActorIdApp ActorIdApp;
        public static RealmApp RealmApp;
        public static GateApp GateApp;
        public static MapApp MapApp;
        public static Dictionary<Type, List<long>> AppIds = new();
        public static Dictionary<long, AppConfig> AppConfigs = new();


        public static void Start(Root root, string serverType)
        {
            Root = root;
            RootScene = root;
            root.AddComponent<ServerAppTypeComponent, string>(serverType);
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
            var app = RootScene.AddChildWithId(Type.GetType($"AO.{appConfig.Type}"), appConfig.Id, instanceId);

            AppConfigs.Add(appConfig.Id, appConfig);
            AppIds.Add(app.GetType(), new List<long> { app.InstanceId });
            app.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);
            if (app is ActorIdApp) ActorIdApp = (ActorIdApp)app;
            if (app is RealmApp) RealmApp = (RealmApp)app;
            if (app is GateApp) GateApp = (GateApp)app;
            if (app is MapApp) MapApp = (MapApp)app;

            if (app is MapApp) DomainApp = MapApp;
        }

        public static AppConfig GetAppConfig(long id)
        {
            return AppConfigs[id];
        }

        public static long GetAppId<T>(int index = 0) where T : IApp
        {
            return AppIds[typeof(T)][index];
        }

        public static IApp? GetAppCall(long appInstanceId)
        {
            return ETRoot.Instance.Get(appInstanceId) as IApp;
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