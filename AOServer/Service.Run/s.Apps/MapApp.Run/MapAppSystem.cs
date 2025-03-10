﻿namespace AO
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

                var actor = ActorFactory.Create(ActorType.NonPlayer, map1Scene);
                map1Scene.GetComponent<SceneUnitComponent>().Add(actor);

                // 向中心世界服注册场景id
                var registerMapMsg = new RegisterMapSceneRequest() { MapType = map1Scene.Type, SceneId = map1Scene.InstanceId };
                AOZone.GetAppCall<WorldServiceAppCall>().RegisterMapSceneRequest(registerMapMsg).Coroutine();

                TestRunEvent().Coroutine();
            }
        }

        private static async ETTask TestRunEvent()
        {
            await TimerComponent.Instance.WaitAsync(1000);
            Process_UnitDeadProcess.Execute(new Actor(), new Actor());
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