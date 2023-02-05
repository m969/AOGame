namespace AO
{
    using AO;
    using ET;
    using ET.Server;
    using System.Text.Json;

    public static class ServerAppTypeComponentSystem
    {
        [ObjectSystem]
        public class ServerAppTypeComponentAwakeSystem : AwakeSystem<ServerAppTypeComponent, string>
        {
            protected override void Awake(ServerAppTypeComponent self, string serverType)
            {
                self.ServerType = serverType;
                var rootScene = self.Parent;
                if (serverType == "AllInOneServer")
                {
                    var appConfig = new AppConfig() { IP = "127.0.0.1" };
                    appConfig.Type = "ActorIdApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 11;
                    //var actorIdApp = rootScene.AddChildWithId<ActorIdApp>(1, 1);
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = "127.0.0.1" };
                    appConfig.Type = "RealmApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 101;
                    //var realmApp = rootScene.AddChildWithId<RealmApp>(101, 101);
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = "127.0.0.1" };
                    appConfig.Type = "GateApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 102;
                    //var gateApp = rootScene.AddChildWithId<GateApp>(102, 102);
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = "127.0.0.1" };
                    appConfig.Type = "MapApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 103;
                    //var mapApp = rootScene.AddChildWithId<MapApp>(103, 103);
                    AOGame.InstallApp(appConfig);
                }
                else
                {
                    // 分布式暂不支持

                    //var types = AssemblyHelper.GetAssemblyTypes(typeof(GateApp).Assembly);
                    //Type appType = null;
                    //foreach ( var item in types)
                    //{
                    //    if (item.Key == serverType)
                    //    {
                    //        appType = item.Value;
                    //        break;
                    //    }
                    //}
                    //if (appType == null)
                    //{
                    //    return;
                    //}
                    //rootScene.AddComponent(appType);
                }

                var appLog = "(";
                foreach (var entity in rootScene.Children.Values)
                {
                    if (entity is IApp app)
                    {
                        appLog += $"PID:{Options.Instance.Process} {app.GetType().Name.ToLower()}, ";
                    }
                }
                appLog = appLog.TrimEnd(' ');
                appLog = appLog.TrimEnd(',');
                appLog += ")";
                Log.Console(appLog);

                LoadTablesData();
            }

            static void LoadTablesData()
            {
                var tables = new cfg.Tables(LoadJson);
                Console.WriteLine("Tables == load succ ==");

                Log.Console($"{tables.TbItems.Get(10000).Desc}");
            }

            private static JsonElement LoadJson(string file)
            {
                return JsonDocument.Parse(System.IO.File.ReadAllBytes("../../ExcelTablesData/" + file + ".json")).RootElement;
            }
        }
    }
}