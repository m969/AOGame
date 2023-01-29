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
        protected override async ETTask Run(Scene scene, EventType.EntryEvent1 args)
        {
            Root.Instance.Scene.AddComponent<NetThreadComponent>();
            Root.Instance.Scene.AddComponent<OpcodeTypeComponent>();
            Root.Instance.Scene.AddComponent<MessageDispatcherComponent>();
            //Root.Instance.Scene.AddComponent<NumericWatcherComponent>();
            //Root.Instance.Scene.AddComponent<AIDispatcherComponent>();
            //Root.Instance.Scene.AddComponent<ClientSceneManagerComponent>();
            // 发送普通actor消息
            Root.Instance.Scene.AddComponent<ActorMessageSenderComponent>();
            // 发送location actor消息
            Root.Instance.Scene.AddComponent<ActorLocationSenderComponent>();
            // 访问location server的组件
            Root.Instance.Scene.AddComponent<LocationProxyComponent>();
            Root.Instance.Scene.AddComponent<ActorMessageDispatcherComponent>();

            if (args.args == "server")
            {
                var ipend = IPEndPoint.Parse("127.0.0.1:11001");
                if (false)
                {
                    ipend = IPEndPoint.Parse("192.168.1.101:11001");
                }
                Root.Instance.Scene.AddComponent<NetServerComponent, IPEndPoint>(ipend);
                ipend = IPEndPoint.Parse("127.0.0.1:22001");
                Root.Instance.Scene.AddComponent<NetInnerComponent, IPEndPoint>(ipend);
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