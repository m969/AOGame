namespace AO
{
    using AO;
    using ET;
    using ET.Server;
    using System.Text.Json;

    public static class AppTypeComponentSystem
    {
        [ObjectSystem]
        public class AwakeHandler : AwakeSystem<AppTypeComponent, string>
        {
            protected override void Awake(AppTypeComponent self, string appType)
            {
                self.AppType = appType;

                LoadTablesData();

                var rootScene = self.Parent;
                var appConfig = new AppConfig() { IP = "127.0.0.1", Port = 22001 };
                appConfig.Type = appType;
                appConfig.Id = 1;
                AOGame.InstallApp(appConfig);

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
            }

            private static void LoadTablesData()
            {
                var tables = new cfg.Tables(LoadJson);
                System.Console.WriteLine("Tables == load succ ==");
                CfgTables.Tables = tables;

                //Log.Console($"{tables.TbItems.Get(10000).Desc}");
            }

            private static JsonElement LoadJson(string file)
            {
                return JsonDocument.Parse(System.IO.File.ReadAllBytes("../../ExcelTablesData/" + file + ".json")).RootElement;
            }
        }
    }
}