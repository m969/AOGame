namespace ET
{
    using ET;
    using AO;
    using System.Net;
    using System.Net.Sockets;
    using Puerts;
    using UnityEngine;

    [Event(SceneType.Process)]
    public class EntryEvent2_InitShare: AEvent<EventType.EntryEvent2>
    {
        protected override async ETTask Run(Scene scene, EventType.EntryEvent2 args)
        {
            Log.Debug("EntryEvent2_InitShare Run");

            AOGame.Start(Root.Instance.Scene);

            await TimerComponent.Instance.WaitAsync(1000);

            await ETTask.CompletedTask;
        }
    }
}