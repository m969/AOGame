using ET;
using SimpleJSON;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

                if (SceneManager.GetActiveScene().name == "Init")
                {
                    self.AddComponent<LoginModeComponent>();
                }
                else if (SceneManager.GetActiveScene().name == "ExecutionLinkScene")
                {
                    self.AddComponent<MapModeComponent>();
                    self.AddComponent<ExecutionEditorModeComponent>();
                }

                var tables = new cfg.Tables(LoadByteBuf);
                var itemcfg = tables.TbItems.Get(10000);
                CfgTables.Tables = tables;
                Log.Console($"{itemcfg.Desc}");
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