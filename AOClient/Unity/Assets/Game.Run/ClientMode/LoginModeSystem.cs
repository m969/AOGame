using ET;
using Puerts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ET.DisposeActionComponent;

namespace AO
{
    public static class LoginModeSystem
    {
        [ObjectSystem]
        public class LoginModeComponentAwakeSystem : AwakeSystem<LoginModeComponent>
        {
            protected override void Awake(LoginModeComponent self)
            {

            }
        }

        /// <summary> µÇÂ¼ </summary>
        public static async Task Login(this LoginModeComponent self)
        {
            await ServerCall.C2G_LoginGate(new C2G_LoginGate() { Key = 101 });
            AOGame.ClientApp.AddComponent<LoadingModeComponent>();
            AOGame.ClientApp.RemoveComponent<LoginModeComponent>();
            AOGame.ClientApp.AddComponent<LobbyModeComponent>();
            await TimerComponent.Instance.WaitAsync(1500);
            AOGame.ClientApp.RemoveComponent<LoadingModeComponent>();
        }
    }
}