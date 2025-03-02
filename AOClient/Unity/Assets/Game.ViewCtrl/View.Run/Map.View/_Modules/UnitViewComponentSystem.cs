using AssetFile;
using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TComp = AO.UnitViewComponent;

namespace AO
{
    public static class UnitViewComponentSystem
    {
        public class AwakeSystemObject : AwakeSystem<TComp>
        {
            protected override async void Awake(TComp self)
            {
                var assetName = string.Empty;
                if (self.Parent is Actor actor)
                {
                    if (actor.ActorType == ActorType.Player)
                    {
                        assetName = "Hero.prefab";
                    }
                    else
                    {
                        assetName = "Enemy.prefab";
                    }
                }
                //if (self.Parent is NpcUnit)
                //{
                //    assetName = "Enemy.prefab";
                //}
                if (self.Parent is ItemUnit)
                {
                    assetName = "ItemUnit.prefab";
                }
                var asset = AssetUtils.LoadAssetWithParentAsync(assetName, self);
                //self.AddComponent(asset);
                await asset.LoadAsync();
                //await asset.Task;
                self.CreateViewObj(asset);

                //Log.Debug($"UnitViewComponentSystem {self.Parent.MapUnit().Name}");
                if (self.Parent.MapUnit().Name == "Execution_1008_Expllosion")
                {
                    var renderAsset = AssetUtils.LoadAssetWithParentAsync("Explosion.prefab", null);
                    await renderAsset.LoadAsync();
                    //await renderAsset.Task;
                    var renderObj = GameObject.Instantiate(renderAsset.GameObjectPrefab, self.UnitObj.transform);
                    renderObj.transform.localPosition = Vector3.zero;
                    self.DestroyWithComponent = false;
                    GameObject.Destroy(self.UnitObj, 2f);
                }
                if (self.Parent.MapUnit().Name == "Execution_1002")
                {
                    var renderAsset = AssetUtils.LoadAssetWithParentAsync("Fire.prefab", null);
                    await renderAsset.LoadAsync();
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
                if (self.DestroyWithComponent)
                {
                    GameObject.Destroy(self.UnitObj);
                }
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