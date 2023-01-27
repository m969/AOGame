using Puerts;
using System;

namespace ET
{
    public class ClientApp : Entity, IAwake, IAddComponent
    {
        public delegate void ModuleCallback(string type, string arg);
        public JsEnv jsEnv;
        public ModuleCallback JsCallback;
    }
}