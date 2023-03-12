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
            protected override async void Awake(TComp self)
            {
                //Log.Console(self.GetType().Name);
                self.AddComponent<MapSceneComponent>();

                EGamePlay.Entity.EnableLog = false;
                EGamePlay.MasterEntity.Create();
                EGamePlay.MasterEntity.Instance.AddChild<CombatContext>();


                var sceneComp = self.GetComponent<MapSceneComponent>();
                var map1Scene = sceneComp.AddChild<Scene, string>("Map1");
                sceneComp.Add(map1Scene);

                var monster = map1Scene.AddChild<NpcUnit>();
                map1Scene.GetComponent<SceneUnitComponent>().Add(monster);

                await TimerComponent.Instance.WaitAsync(1000);
                //monster.Cache();
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