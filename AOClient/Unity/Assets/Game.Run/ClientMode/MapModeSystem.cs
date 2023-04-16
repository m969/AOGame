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
    public static class MapModeSystem
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
            var loadingMode = AOGame.ClientApp.GetOrAdd<LoadingModeComponent>();
            self.RemoveCurrentScene();
            var mapScene = self.CreateMapScene(map);
            Scene.CurrentScene = mapScene;
            var asset = AssetUtils.LoadAssetWithParentAsync($"{mapScene.Type}.unity", mapScene);
            var op = asset.LoadSceneAsync();
            while (!op.isDone)
            {
                await TimerComponent.Instance.WaitAsync(30);
                loadingMode.SetProgressValue(op.progress * 100);
            }
            await TimerComponent.Instance.WaitAsync(1000);
            AOGame.ClientApp.RemoveComponent<LoadingModeComponent>();
        }

        public static void RemoveCurrentScene(this TComp self)
        {
            if (Scene.CurrentScene != null)
            {
                self.GetComponent<MapSceneComponent>().Remove(Scene.CurrentScene.Id);
                Scene.CurrentScene?.Dispose();
                Scene.CurrentScene = null;
            }
        }

        public static Scene CreateMapScene(this TComp self, string map)
        {
            var sceneComp = self.GetComponent<MapSceneComponent>();
            var mapScene = sceneComp.AddChild<Scene, string>(map);
            sceneComp.Add(mapScene);
            return mapScene;
        }
    }
}