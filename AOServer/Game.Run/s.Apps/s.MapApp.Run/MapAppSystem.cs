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
                sceneComp.Add(map1Scene);

                ActorFactory.CreateActor(ActorType.Npc, map1Scene);

                //var monster = map1Scene.AddChild<NpcUnit>();
                //map1Scene.GetComponent<SceneUnitComponent>().Add(monster);

                // 向中心世界服注册场景id
                var registerMapMsg = new RegisterMapSceneRequest() { MapType = map1Scene.Type, SceneId = map1Scene.InstanceId };
                AOZone.GetAppCall<WorldServiceAppCall>().RegisterMapSceneRequest(registerMapMsg).Coroutine();
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