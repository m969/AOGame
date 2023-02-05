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

                var sceneComp = self.GetComponent<MapSceneComponent>();
                var map1Scene = EntitySceneFactory.CreateScene(1, SceneType.Map, "map1", AOGame.RootScene);
                map1Scene.AddComponent<SceneUnitComponent>();
                sceneComp.Add(map1Scene);

                var enemy = map1Scene.AddChild<EnemyUnit>();
                enemy.AddComponent<UnitTranslateComponent>();
                enemy.AddComponent<UnitPathMoveComponent>();
                enemy.AddComponent<LevelComponent>();
                map1Scene.GetComponent<SceneUnitComponent>().Add(enemy);
            }
        }
    }
}