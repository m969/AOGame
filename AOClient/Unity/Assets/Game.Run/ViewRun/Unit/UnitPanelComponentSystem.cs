using AssetFile;
using ET;
using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using TComp = AO.UnitPanelComponent;

namespace AO
{
    public static class UnitPanelComponentSystem
    {
        public class AwakeSystemObject : AwakeSystem<TComp>
        {
            protected override async void Awake(TComp self)
            {
                var asset = AssetUtils.LoadAssetAsync("UnitPanel.prefab");
                await asset.Task;
                self.CreatePanelObj(asset);
            }
        }

        public class DestroySystemObject : DestroySystem<TComp>
        {
            protected override void Destroy(TComp self)
            {
                if (self.UnitPanel != null)
                {
                    GameObject.Destroy(self.UnitPanel.gameObject);
                }
            }
        }

        public class UpdateSystemObject : UpdateSystem<TComp>
        {
            protected override void Update(TComp self)
            {
                if (self.UnitPanel != null)
                {
                    self.UnitPanel.transform.position = self.Parent.MapUnit().Position;
                }
            }
        }

        public static void CreatePanelObj(this TComp self, Asset asset)
        {
            var prefab = asset.GameObjectPrefab;
            var obj = GameObject.Instantiate(prefab);
            GameObject.DontDestroyOnLoad(obj);
            var uipanel = obj.GetComponentInChildren<UIPanel>();
            uipanel.packageName = "Common";
            uipanel.componentName = "HealthBar";
            uipanel.CreateUI();
            obj.transform.position = self.Parent.MapUnit().Position;
        }
    }
}