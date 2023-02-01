namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static class MapAppSystem
    {
        [ObjectSystem]
        public class MapAppAwakeSystem : AwakeSystem<MapApp>
        {
            protected override void Awake(MapApp self)
            {
                //Log.Console(self.GetType().Name);
                self.AddComponent<MapSceneComponent>();
                //self.AddComponent<MapUnitComponent>();
            }
        }
    }
}