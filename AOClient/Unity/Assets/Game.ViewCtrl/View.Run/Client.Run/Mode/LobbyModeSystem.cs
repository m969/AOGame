using ET;
using Puerts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AO
{
    public static class LobbyModeSystem
    {
        [ObjectSystem]
        public class LobbyModeComponentAwakeSystem : AwakeSystem<LobbyModeComponent>
        {
            protected override void Awake(LobbyModeComponent self)
            {

            }
        }

        /// <summary> �����ͼ </summary>
        public static async Task EnterMap(this LobbyModeComponent self)
        {
            await PlayerCall.C2G_EnterMap(new C2G_EnterMap());
        }
    }
}