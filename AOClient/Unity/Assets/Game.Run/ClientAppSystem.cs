using ET;
using System;
using System.Collections;
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
                AOGame.ClientApp = self;
                self.AddComponent<LoginModeComponent>();
                self.AddComponent<MapSceneComponent>();
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