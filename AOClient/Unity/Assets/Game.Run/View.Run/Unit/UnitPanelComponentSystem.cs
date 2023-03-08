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
                    self.UnitPanel.transform.parent.position = self.Parent.MapUnit().Position;
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
            self.UnitPanel = uipanel;

            self.SetHPMax(100);
            self.SetHP(100);
        }

        public static void SetHPMax(this TComp self, int hp)
        {
            if (self.UnitPanel == null) 
            {
                return;
            }
            self.UnitPanel.ui.asProgress.max = hp;
        }

        public static void SetHP(this TComp self, int hp)
        {
            if (self.UnitPanel == null)
            {
                return;
            }
            self.UnitPanel.ui.asProgress.value = hp;
            Log.Console($"SetHP {self.UnitPanel.ui.asProgress.value}/{self.UnitPanel.ui.asProgress.max}");
        }
    }
}