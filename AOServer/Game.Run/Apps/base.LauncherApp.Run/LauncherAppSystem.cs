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