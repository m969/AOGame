namespace ET
{
    using ET;
    using AO;
    using System.Net;
    using ET.Client;
    using System.Net.Sockets;
    using System;

    [Event(SceneType.Process)]
    public class EntryEvent1_InitShare: AEvent<EventType.EntryEvent1>
    {
        protected override async ETTask Run(Entity source, EventType.EntryEvent1 args)
        {
            Log.Debug("EntryEvent1_InitShare Run");
            ETRoot.Root.AddComponent<NetThreadComponent>();
            ETRoot.Root.AddComponent<OpcodeTypeComponent>();
            ETRoot.Root.AddComponent<MessageDispatcherComponent>();
            //Root.Instance.Scene.AddComponent<NumericWatcherComponent>();
            //Root.Instance.Scene.AddComponent<AIDispatcherComponent>();
            //Root.Instance.Scene.AddComponent<ClientSceneManagerComponent>();

            ETRoot.Root.AddComponent<NetClientComponent, AddressFamily>(AddressFamily.InterNetwork);
            await ETTask.CompletedTask;

            OpcodeHelper.ignoreDebugLogMessageSet.Add(OuterMessage.C2M_PathfindingResult);
            OpcodeHelper.ignoreDebugLogMessageSet.Add(OuterMessage.M2C_PathfindingResult);

            AO.EventType.RequestCall.CallAction = CallAction;
            AO.EventType.RequestCall.SendAction = SendAction;
        }

        public static Session GetSession()
        {
            var clientApp = AOGame.ClientApp;
            if (clientApp.GetComponent<SessionComponent>() == null)
            {
                var serverAddress = "127.0.0.1:11001";
                if (!Define.IsEditor)
                {
                    serverAddress = "192.168.1.101:11001";
                }
                var tempSession = AOGame.RootScene.GetComponent<NetClientComponent>().Create(NetworkHelper.ToIPEndPoint(serverAddress));
                clientApp.AddComponent<SessionComponent>().Session = tempSession;
            }
            var session = clientApp.GetComponent<SessionComponent>().Session;
            return session;
        }

        public static void SendAction(IMessage msg)
        {
            GetSession().Send(msg);
        }

        public static async void CallAction(AO.EventType.RequestCall msgCall)
        {
            var response = await GetSession().Call(msgCall.Request);
            msgCall.Response = response;
            msgCall.Task.SetResult(response);
            //Log.Debug($"CallAction {msgCall.Request.GetType()} {msgCall.Response.GetType()}");
        }
    }
}