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
        public class AwakeSystemObject : AwakeSystem<TComp, Asset>
        {
            protected override async void Awake(TComp self, Asset asset)
            {
                self.AddComponent(asset);
                await asset.Task;
                self.CreateViewObj(asset);
                Log.Debug($"UnitViewComponentSystem {self.Parent.MapUnit().Name}");
                if (self.Parent.MapUnit().Name == "Execution_1008_Expllosion")
                {
                    var renderAsset = AssetUtils.LoadAssetAsync("Ñ×±¬ÌØÐ§.prefab");
                    await renderAsset.Task;
                    var renderObj = GameObject.Instantiate(renderAsset.GameObjectPrefab, self.UnitObj.transform);
                    renderObj.transform.localPosition = Vector3.zero;
                }
                if (self.Parent.MapUnit().Name == "Execution_1002")
                {
                    var renderAsset = AssetUtils.LoadAssetAsync("Fire.prefab");
                    await renderAsset.Task;
                    var renderObj = GameObject.Instantiate(renderAsset.GameObjectPrefab, self.UnitObj.transform);
                    renderObj.transform.localPosition = Vector3.zero;
                }
            }
        }

        public class DestroySystemObject : DestroySystem<TComp>
        {
            protected override void Destroy(TComp self)
            {
                //Log.Debug("UnitViewComponent Destroy");
                GameObject.Destroy(self.UnitObj);
            }
        }

        public static void CreateViewObj(this TComp self, Asset asset)
        {
            var prefab = asset.GameObjectPrefab;
            var obj = GameObject.Instantiate(prefab);
            GameObject.DontDestroyOnLoad(obj);
            self.UnitObj = obj;
            obj.transform.position = self.Parent.MapUnit().Position;
            self.Parent.AddComponent<UnitAnimationComponent>();
        }
    }
}