namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static class GateAppSystem
    {
        [ObjectSystem]
        public class GateAppAwakeSystem : AwakeSystem<GateApp>
        {
            protected override void Awake(GateApp self)
            {
                //Log.Console(self.GetType().Name);
                self.AddComponent<PlayerComponent>();
            }
        }
    }
}