﻿using AO;
using Puerts;
using System;

namespace ET
{
    public class ClientApp : Entity, IAwake, IAddComponent, IApp, IUpdate, IDestroy
    {
        public delegate void ModuleCallback(string type, object arg);
        public JsEnv jsEnv;
        public ModuleCallback JsCallback;

        public int DomainIndex => Zone;
        /// <summary>
        /// 区
        /// </summary>
        public int Zone
        {
            get; set;
        }
    }
}