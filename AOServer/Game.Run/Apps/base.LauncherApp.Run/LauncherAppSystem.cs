namespace AO
{
    using AO;
    using ET;
    using ET.Server;
    using GameUtils;
    using System.IO;

    public static class LauncherAppSystem
    {
        public class AwakeHandler : AwakeSystem<LauncherApp>
        {
            protected override void Awake(LauncherApp self)
            {
                //Log.Console(self.GetType().Name);
                //self.AddComponent<LocationComponent>();

                var localIp = "127.0.0.1";
                var launcherType = ET.Options.Instance.LauncherType;
                if (launcherType == "AllInOneServer")
                {
                    // 本地单进程服务器模式

                    var appConfig = new AppConfig() { IP = localIp };
                    appConfig.Type = "ActorIdApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 31;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = localIp, Port = 22001, DBConnection = "mongodb://127.0.0.1" };
                    appConfig.Type = "DBConnectApp";
                    appConfig.Id = 11;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = localIp, Port = 22001 };
                    appConfig.Type = "DBCacheApp";
                    appConfig.Id = 21;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = localIp };
                    appConfig.Type = "RealmApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 101;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = localIp };
                    appConfig.Type = "GateApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 102;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = localIp };
                    appConfig.Type = "MapApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 103;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = localIp };
                    appConfig.Type = nameof(WorldServiceApp);
                    appConfig.Port = 22001;
                    appConfig.Id = 104;
                    AOGame.InstallApp(appConfig);
                }

                if (launcherType == "AllInZoneServer")
                {
                    // 分布式区服进程模式暂未实现（一个区服一个进程）

                    var appConfig = new AppConfig() { IP = localIp, DBConnection = "mongodb://127.0.0.1" };
                    appConfig.Type = "DBConnectApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 11;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = localIp };
                    appConfig.Type = "GateApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 102;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = localIp };
                    appConfig.Type = "MapApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 103;
                    AOGame.InstallApp(appConfig);
                }

                if (launcherType == "Distribution")
                {
                    // 分布式应用进程模式暂未实现（一个App一个进程）
                    return;

                    var machineIp = "192.168.0.1";
                    var appConfigs = File.ReadAllLines($"../ServerConfigs/MyServerApps/{machineIp}.txt");
                    foreach (var item in appConfigs)
                    {
                        var arr = item.Split(':');
                        var pid = int.Parse(arr[0]);
                        var appType = arr[1];
                        var appPort = int.Parse(arr[2]);
                        ProcessHelper.Run("dotnet.exe", item);
                    }
                }
            }
        }
    }
}