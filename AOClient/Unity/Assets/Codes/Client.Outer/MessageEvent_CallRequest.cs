//namespace AO
//{
//    using ET;
//    using AO;
//    using System.Net;
//    using ET.Client;
//    using System.Net.Sockets;
//    using System;

//    [Event(SceneType.Process)]
//    public class MessageEvent_CallRequest : AEvent<EventType.RequestCallEvent>
//    {
//        protected override async ETTask Run(Scene scene, EventType.RequestCallEvent args)
//        {
//            if (AOGame.ClientApp.GetComponent<SessionComponent>() == null)
//            {
//                var tempSession = AOGame.RootScene.GetComponent<NetClientComponent>().Create(NetworkHelper.ToIPEndPoint("127.0.0.1:11001"));
//                AOGame.ClientApp.AddComponent<SessionComponent>().Session = tempSession;
//            }
//            var session = AOGame.ClientApp.GetComponent<SessionComponent>().Session;

//            var response = await session.Call(args.RequestCall.Request);
//            args.RequestCall.Response = response;
//            Log.Debug($"MessageEvent_CallRequest {args.RequestCall.Request.GetType()} {args.RequestCall.Response.GetType()}");
//        }
//    }
//}