using AO;
using Puerts;
using System;

namespace ET
{
    public class ClientApp : Entity, IAwake, IAddComponent, IApp
    {
        public delegate void ModuleCallback(string type, string arg);
        public JsEnv jsEnv;
        public ModuleCallback JsCallback;

        public int DomainIndex => Zone;
        /// <summary>
        /// 区
        /// </summary>
        public int Zone
        {
            get;
        }
    }
}