namespace AO
{
    using ET;
    using ET.Server;
    using ActorSendEvent = AO.EventType.ActorSendEvent;
    using BroadcastEvent = AO.EventType.BroadcastEvent;

    public class PlayerClientAwakeSystem: AwakeSystem<PlayerClient, long>
    {
        protected override void Awake(PlayerClient self, long sessionId)
        {
            self.Parent.AddComponent<GateSessionIdComponent, long>(sessionId);
            self.Parent.AddComponent<MailBoxComponent>();
            self.Client = new PlayerClient.ClientCall();
            self.Client.SessionId = sessionId;
            self.AOIClients = new PlayerClient.AOICall();
            self.AOIClients.Unit = self.Parent as IMapUnit;
        }
    }

    public class PlayerClientDestroyHandler : DestroySystem<PlayerClient>
    {
        protected override void Destroy(PlayerClient self)
        {
            self.Parent.RemoveComponent<GateSessionIdComponent>();
            self.Parent.RemoveComponent<MailBoxComponent>();
        }
    }

    public class PlayerClient : Entity, IAwake<long>, IDestroy
    {
        public ClientCall Client { get; set; }
        public AOICall AOIClients { get; set; }

        public class ClientCall
        {
            public long SessionId { get; set; }

        }
        public class AOICall
        {
            public IMapUnit Unit { get; set; }

        }
    }
}