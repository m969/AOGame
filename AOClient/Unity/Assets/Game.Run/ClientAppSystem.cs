using ET;
using SimpleJSON;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace AO
{
    public static class ClientAppSystem
    {
        [ObjectSystem]
        public class ClientAppAwakeSystem : AwakeSystem<ClientApp>
        {
            protected override void Awake(ClientApp self)
            {
                Log.Debug("ClientAppAwakeSystem Awake");

                AOGame.ClientApp = self;

                self.AddComponent<LoginModeComponent>();
                self.AddComponent<MapSceneComponent>();

                var tables = new cfg.Tables(LoadByteBuf);
                var itemData = tables.TbItems.Get(10000);
                Log.Console($"{itemData.Desc}");
            }

            private static JSONNode LoadByteBuf(string file)
            {
                return JSON.Parse(File.ReadAllText(Application.dataPath + "/Bundles/ExcelTablesData/" + file + ".json", System.Text.Encoding.UTF8));
            }
        }

        [ObjectSystem]
        public class ClientAppAddComponentSystem : AddComponentSystem<ClientApp>
        {
            protected override void AddComponent(ClientApp self, Entity component)
            {
                if (component is IClientMode)
                {
                    var modeName = component.GetType().Name.Replace("Component", "");
                    self.JsCallback?.Invoke($"{modeName}_Enter", string.Empty);
                }
            }
        }
    }
} 