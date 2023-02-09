namespace AO
{
    using AO;
    using EGamePlay.Combat;
    using ET;
    using ET.Server;
    using UnityEngine;
    using TComp = AO.MapApp;

    public static class MapAppSystem
    {
        [ObjectSystem]
        public class MapAppAwakeSystem : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                //Log.Console(self.GetType().Name);
                self.AddComponent<MapSceneComponent>();

                EGamePlay.Entity.EnableLog = false;
                EGamePlay.MasterEntity.Create();
                EGamePlay.MasterEntity.Instance.AddChild<CombatContext>();


                var sceneComp = self.GetComponent<MapSceneComponent>();
                var map1Scene = sceneComp.AddChild<Scene, string>("Map1");
                //var map1Scene = EntitySceneFactory.CreateScene(1, SceneType.Map, "map1", AOGame.RootScene);
                map1Scene.AddComponent<SceneUnitComponent>();
                sceneComp.Add(map1Scene);

                var enemy = map1Scene.AddChild<EnemyUnit>();
                map1Scene.GetComponent<SceneUnitComponent>().Add(enemy);
            }
        }

        [ObjectSystem]
        public class MapAppUpdateSystem : UpdateSystem<TComp>
        {
            protected override void Update(TComp self)
            {
                EGamePlay.MasterEntity.Instance.Update();
            }
        }
    }
}