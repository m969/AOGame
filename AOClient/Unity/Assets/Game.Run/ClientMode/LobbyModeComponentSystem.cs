using ET;
using Puerts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AO
{
    public static class LobbyModeComponentSystem
    {
        [ObjectSystem]
        public class LobbyModeComponentAwakeSystem : AwakeSystem<LobbyModeComponent>
        {
            protected override void Awake(LobbyModeComponent self)
            {

            }
        }

        /// <summary> ½øÈëµØÍ¼ </summary>
        public static async Task EnterMap(this LobbyModeComponent self)
        {
            await PlayerCall.C2G_EnterMap(new C2G_EnterMap());

            AOGame.TryGet(out MapSceneComponent sceneComp);
            var map1Scene = MapSceneComponentSystem.GetScene(sceneComp, "map1");
            if (map1Scene == null)
            {
                map1Scene = EntitySceneFactory.CreateScene(1, SceneType.Map, "map1", AOGame.RootScene);
                MapSceneComponentSystem.Add(sceneComp, map1Scene);
                map1Scene.AddComponent<SceneUnitComponent>();
            }

            await AssetUtils.LoadSceneAsync("Map1.unity");

            AOGame.ClientApp.RemoveComponent<LobbyModeComponent>();
            //await TimerComponent.Instance.WaitAsync(300);
            AOGame.ClientApp.AddComponent<MapModeComponent>();
        }
    }
}