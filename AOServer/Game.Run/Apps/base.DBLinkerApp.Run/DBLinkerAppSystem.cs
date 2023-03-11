namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static class DBLinkerAppSystem
    {
        public class AwakeHandler : AwakeSystem<DBLinkerApp>
        {
            protected override void Awake(DBLinkerApp self)
            {
                //Log.Console(self.GetType().Name);
                self.AddComponent<DBManagerComponent>();
            }
        }
    }
}