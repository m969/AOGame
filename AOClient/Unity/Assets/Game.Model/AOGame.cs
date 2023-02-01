using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AO
{
	public class AOGame
	{
        public static Scene RootScene;
        public static ClientApp ClientApp;

        public static void Start(Scene rootScene)
        {
            RootScene = rootScene;
            rootScene.AddComponent<ClientApp>();
        }

        public static void Run(Entity app)
        {
            
        }

        public static bool TryGet<T>(out T comp) where T : Entity
        {
            comp = ClientApp.GetComponent<T>();
            return comp != null;
        }

        public static async ETTask PublishAsync<T>(T a) where T : struct
        {
            await EventSystem.Instance.PublishAsync(RootScene, a);
        }

        public static void Publish<T>(T a) where T : struct
        {
            EventSystem.Instance.Publish(RootScene, a);
        }
    }
}