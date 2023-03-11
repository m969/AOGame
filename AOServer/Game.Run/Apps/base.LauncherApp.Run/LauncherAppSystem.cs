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

                var launcherType = ET.Options.Instance.LauncherType;
                if (launcherType == "AllInOneServer")
                {
                    var appConfig = new AppConfig() { IP = "127.0.0.1", Port = 22001, DBConnection = "mongodb://127.0.0.1" };
                    appConfig.Type = "DBLinkerApp";
                    appConfig.Id = 11;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = "127.0.0.1", Port = 22001 };
                    appConfig.Type = "DBCacheApp";
                    appConfig.Id = 21;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = "127.0.0.1" };
                    appConfig.Type = "ActorIdApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 31;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = "127.0.0.1" };
                    appConfig.Type = "RealmApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 101;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = "127.0.0.1" };
                    appConfig.Type = "GateApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 102;
                    AOGame.InstallApp(appConfig);

                    appConfig = new AppConfig() { IP = "127.0.0.1" };
                    appConfig.Type = "MapApp";
                    appConfig.Port = 22001;
                    appConfig.Id = 103;
                    AOGame.InstallApp(appConfig);
                }
                else
                {
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