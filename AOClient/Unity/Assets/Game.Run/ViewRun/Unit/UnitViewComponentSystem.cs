using AssetFile;
using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using TComp = AO.UnitViewComponent;

namespace AO
{
    public static class UnitViewComponentSystem
    {
        [ObjectSystem]
        public class UnitViewComponentAwakeSystem : AwakeSystem<TComp, Asset>
        {
            protected override async void Awake(TComp self, Asset asset)
            {
                self.AddComponent(asset);
                await asset.Task;
                self.CreateViewObj(asset);
            }
        }

        public static void CreateViewObj(this TComp self, Asset asset)
        {
            var prefab = asset.GameObject;
            var obj = GameObject.Instantiate(prefab);
            GameObject.DontDestroyOnLoad(obj);
            self.UnitObj = obj;
            obj.transform.position = self.Parent.MapUnit().Position;
            //obj.transform.forward = unitInfo.Forward;
            self.Parent.AddComponent<UnitAnimationComponent>();
        }
    }
}