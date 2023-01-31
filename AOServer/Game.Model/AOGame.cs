namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static class AOGame
    {
        public static Root Root;
        public static Scene RootScene;
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
            RootScene = root.Scene;
            root.Scene.AddComponent<ServerTypeComponent, string>(serverType);
        }

        public static void Run()
        {
            //game.
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
            return Root.Instance.Get(appInstanceId) as IApp;
        }

        public static async ETTask PublishAsync<T>(T a) where T : struct
        {
            await EventSystem.Instance.PublishAsync(RootScene, a);
        }

        public static void Publish<T>(T a) where T : struct
        {
            EventSystem.Instance.Publish(RootScene, a);
        }
    }
}