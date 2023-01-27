using ET;
using Puerts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static ET.DisposeActionComponent;
using static ET.EntryEvent2_InitShare;

namespace AO
{
    public static class LoginModeComponentSystem
    {
        [ObjectSystem]
        public class LoginModeComponentAwakeSystem : AwakeSystem<LoginModeComponent>
        {
            protected override void Awake(LoginModeComponent self)
            {
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
                        //bootstrapExtension.AppendLine($"require(\"./client_mode/{typeName}/{typeName}_Event.mjs\");");
                        bootstrapExtension.AppendLine($"import * as {typeName}Register from \"./client_mode/{typeName}/{typeName}_Event.mjs\";");
                        bootstrapExtension.AppendLine($"{typeName}Register.register();");
                    }
                }
                bootstrapExtension.AppendLine();
                loader.bootstrapScript = bootstrapExtension.ToString();
                var jsEnv = new JsEnv(loader, 9229);
                var varname = "m_" + Time.frameCount;
                var callback = jsEnv.ExecuteModule<ClientApp.ModuleCallback>("bootstrap.mjs", "callback");
                if (callback != null) callback("init", "");
                Application.runInBackground = true;
                AOGame.ClientApp.jsEnv = jsEnv;
                AOGame.ClientApp.JsCallback = callback;
            }
        }

        /// <summary> µÇÂ¼ </summary>
        public static async Task Login(this LoginModeComponent self)
        {
            //await ServerCall.C2R_Login(new C2R_Login() { Account = "test01", Password = "test01" });
            await ServerCall.C2G_LoginGate(new C2G_LoginGate() { Key = 101 });

            AOGame.ClientApp.RemoveComponent<LoginModeComponent>();
            //await TimerComponent.Instance.WaitAsync(300);
            AOGame.ClientApp.AddComponent<LobbyModeComponent>();
        }
    }
}