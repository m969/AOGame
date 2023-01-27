namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static class ActorIdAppAppSystem
    {
        [ObjectSystem]
        public class ActorIdAppAwakeSystem : AwakeSystem<ActorIdApp>
        {
            protected override void Awake(ActorIdApp self)
            {
                //Log.Console(self.GetType().Name);
                self.AddComponent<LocationComponent>();
            }
        }
    }
}