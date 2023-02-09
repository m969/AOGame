using ET;
using Puerts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TComp = AO.MapModeComponent;

namespace AO
{
    public static class MapModeComponentSystem
    {
        [ObjectSystem]
        public class MapModeComponentAwakeSystem : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                self.AddComponent<MapSceneComponent>();
            }
        }

        /// <summary> ÇÐ»»µØÍ¼³¡¾° </summary>
        public static async Task ChangeMapScene(this TComp self, string map)
        {
            Avatar.CurrentScene?.Dispose();
            Avatar.CurrentScene = null;
            var sceneComp = self.GetComponent<MapSceneComponent>();
            var mapScene = sceneComp.AddChild<Scene, string>("Map1");
            //var mapScene = EntitySceneFactory.CreateScene(1, SceneType.Map, map, AOGame.RootScene);
            sceneComp.Add(mapScene);
            mapScene.AddComponent<SceneUnitComponent>();
            Avatar.CurrentScene = mapScene;
            var asset = await AssetUtils.LoadSceneAsync($"{mapScene.Type}.unity");
            asset.ReleaseWith(mapScene);
        }
    }
}