using ET;
using SimpleJSON;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EGamePlay.Combat;
using Puerts;
using System.Text;
using System.Collections;

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

                

                string root = Application.dataPath + "/Samples/TSBehaviour/Resources";
                var loader = new JsModuleFileLoader(root);
                var bootstrapExtension = new StringBuilder();
                bootstrapExtension.AppendLine("Object.defineProperty(globalThis, 'csharp', { value: require(\"csharp\"), enumerable: true, configurable: false, writable: false });");
                bootstrapExtension.AppendLine("Object.defineProperty(globalThis, 'puerts', { value: require(\"puerts\"), enumerable: true, configurable: false, writable: false });");

                foreach (var item in CodeLoader.Instance.model.GetTypes())
                {
                    if (item.GetInterface("AO.IClientMode") != null)
                    {
                        var typeName = item.Name.Replace("Component", "");
                        var fileName = $"./client_mode/{typeName}/{typeName}_Event.mjs";
                        if (loader.FileExists(fileName))
                        {
                            bootstrapExtension.AppendLine($"import * as {typeName}Register from \"./client_mode/{typeName}/{typeName}_Event.mjs\";");
                            bootstrapExtension.AppendLine($"{typeName}Register.register();");
                        }
                    }
                    var properties = item.GetProperties();
                    foreach (var property in properties)
                    {
                        var attrs = property.GetCustomAttributes(typeof(PropertyChangedAttribute), false);
                        if (attrs.Length > 0)
                        {
                            var typeName = item.Name;
                            var fileName = $"./property_notify/{typeName}_PropertyChanged.mjs";
                            if (loader.FileExists(fileName))
                            {
                                bootstrapExtension.AppendLine($"import * as {typeName}PropertyChangedRegister from \"./property_notify/{typeName}_PropertyChanged.mjs\";");
                                bootstrapExtension.AppendLine($"{typeName}PropertyChangedRegister.register();");
                            }
                            break;
                        }
                    }
                }
                bootstrapExtension.AppendLine();
                loader.bootstrapScript = bootstrapExtension.ToString();
                var jsEnv = new JsEnv(loader, 9229);
                var varname = "m_" + Time.frameCount;
                var callback = jsEnv.ExecuteModule<ClientApp.ModuleCallback>("bootstrap.mjs", "callback");
                if (callback != null) callback("init", null);
                Application.runInBackground = true;
                AOGame.ClientApp.jsEnv = jsEnv;
                AOGame.ClientApp.JsCallback = callback;



                EGamePlay.Entity.EnableLog = false;
                EGamePlay.MasterEntity.Create();
                EGamePlay.MasterEntity.Instance.AddChild<CombatContext>();

                UIUtils.LoadPackageAsync("Common").Coroutine();

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

                // var gameFlowWork = MasterEntity.Instance.AddChild<GameFlowWork>();
                // gameFlowWork.FlowSource.ToEnter<GameInitFlow>().ToEnter<LoginSceneFlow>().ToEnter<CreateSceneFlow>().ToEnter<LobbySceneFlow>().ToEnter<MapSceneFlow>().ToEnd();
                // gameFlowWork.Startup();
            }

            private static JSONNode LoadByteBuf(string file)
            {
                return JSON.Parse(File.ReadAllText(Application.dataPath + "/Bundles/ExcelTablesData/" + file + ".json", System.Text.Encoding.UTF8));
            }
        }

        [ObjectSystem]
        public class ClientAppUpdateSystem : UpdateSystem<ClientApp>
        {
            protected override void Update(ClientApp self)
            {
                EGamePlay.MasterEntity.Instance.Update();
            }
        }

        [ObjectSystem]
        public class ClientAppDestroySystem : DestroySystem<ClientApp>
        {
            protected override void Destroy(ClientApp self)
            {
                EGamePlay.MasterEntity.Destroy();
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
                    self.JsCallback?.Invoke($"{modeName}_Enter", null);
                }
            }
        }
    }
}