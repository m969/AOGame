using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace AO
{
	public class AOGame
	{
        public static Root Root;
        public static Root RootScene;
        public static ClientApp ClientApp;

        public static void Start(Root root)
        {
            Root = root;
            RootScene = root;
            Root.AddComponent<ClientApp>();
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
            await EventSystem.Instance?.PublishAsync(Root, a);
        }

        public static void Publish<T>(T a) where T : struct
        {
            EventSystem.Instance?.Publish(Root, a);
        }

        [Conditional("SERVER")]
        public static void PublishServer<T>(T a) where T : struct
        {
            EventSystem.Instance?.Publish(Root, a);
        }
    }
}