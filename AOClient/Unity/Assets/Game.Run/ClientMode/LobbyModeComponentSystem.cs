using ET;
using Puerts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AO
{
    public static class LobbyModeComponentSystem
    {
        [ObjectSystem]
        public class LobbyModeComponentAwakeSystem : AwakeSystem<LobbyModeComponent>
        {
            protected override void Awake(LobbyModeComponent self)
            {

            }
        }

        /// <summary> ½øÈëµØÍ¼ </summary>
        public static async Task EnterMap(this LobbyModeComponent self)
        {
            await PlayerCall.C2G_EnterMap(new C2G_EnterMap());
            AOGame.ClientApp.RemoveComponent<LobbyModeComponent>();
            AOGame.ClientApp.AddComponent<MapModeComponent>();
        }
    }
}