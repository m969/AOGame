using AO;
using ET;
using Puerts;
using System.Text;
using UnityEngine;

public class Process_PuerTsSetup
{
    public static void Execute(ClientApp clientApp)
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
        clientApp.jsEnv = jsEnv;
        clientApp.JsCallback = callback;
    }
}
