namespace ET
{
    using ET;
    using AO;
    using System.Net;
    using ET.Server;
    using AO;

    [Event(SceneType.Process)]
    public class EntryEvent1_InitShare: AEvent<EventType.EntryEvent1>
    {
        protected override async ETTask Run(Entity source, EventType.EntryEvent1 args)
        {
            ETRoot.Root.AddComponent<NetThreadComponent>();
            ETRoot.Root.AddComponent<OpcodeTypeComponent>();
            ETRoot.Root.AddComponent<MessageDispatcherComponent>();
            //Root.Instance.Scene.AddComponent<NumericWatcherComponent>();
            //Root.Instance.Scene.AddComponent<AIDispatcherComponent>();
            //Root.Instance.Scene.AddComponent<ClientSceneManagerComponent>();
            // 发送普通actor消息
            ETRoot.Root.AddComponent<ActorMessageSenderComponent>();
            // 发送location actor消息
            ETRoot.Root.AddComponent<ActorLocationSenderComponent>();
            // 访问location server的组件
            ETRoot.Root.AddComponent<LocationProxyComponent>();
            ETRoot.Root.AddComponent<ActorMessageDispatcherComponent>();

            OpcodeHelper.ignoreDebugLogMessageSet.Add(OuterMessage.C2M_PathfindingResult);
            OpcodeHelper.ignoreDebugLogMessageSet.Add(OuterMessage.M2C_PathfindingResult);

            if (args.args == "server")
            {
                var ipend = IPEndPoint.Parse("127.0.0.1:11001");
                if (false)
                {
                    ipend = IPEndPoint.Parse("192.168.1.101:11001");
                }
                ETRoot.Root.AddComponent<NetServerComponent, IPEndPoint>(ipend);
                ipend = IPEndPoint.Parse("127.0.0.1:22001");
                ETRoot.Root.AddComponent<NetInnerComponent, IPEndPoint>(ipend);
            }

            //if (args.args == "robot")
            //{
            //    await TimerComponent.Instance.WaitAsync(1000);

            //    var app = ET.Root.Instance.Scene;
            //    //app.AddComponent<NetThreadComponent>();
            //    var ipend = IPEndPoint.Parse("127.0.0.1:33001");
            //    app.AddComponent<NetInnerComponent, IPEndPoint>(ipend);

            //    Session session = app.GetComponent<NetInnerComponent>().CreateInner(1, NetworkHelper.ToIPEndPoint("127.0.0.1:22001"));
            //    session.Send(100, new C2M_TestRequest());
            //    Log.Console("Send C2M_TestRequest");
            //}

            await ETTask.CompletedTask;
        }
    }
}