namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static class DBConnectAppSystem
    {
        public class AwakeHandler : AwakeSystem<DBConnectApp>
        {
            protected override void Awake(DBConnectApp self)
            {
                //Log.Console(self.GetType().Name);
                self.AddComponent<DBManagerComponent>();
            }
        }
    }
}