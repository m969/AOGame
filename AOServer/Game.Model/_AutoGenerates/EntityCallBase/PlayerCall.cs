namespace AO
{
    using ET;
    using ET.Server;
    using ActorSendEvent = AO.EventType.ActorSendEvent;
    using BroadcastEvent = AO.EventType.BroadcastEvent;

    public class PlayerCallAwakeSystem: AwakeSystem<PlayerCall, long>
    {
        protected override void Awake(PlayerCall self, long sessionId)
        {
            self.Parent.AddComponent<GateSessionIdComponent, long>(sessionId);
            self.Parent.AddComponent<MailBoxComponent>();
            self.Client = new PlayerCall.ClientCall();
            self.Client.SessionId = sessionId;
            self.AOIClients = new PlayerCall.AOICall();
            self.AOIClients.Unit = self.Parent as IMapUnit;
        }
    }

    public class PlayerCallDestroyHandler : DestroySystem<PlayerCall>
    {
        protected override void Destroy(PlayerCall self)
        {
            self.Parent.RemoveComponent<GateSessionIdComponent>();
            self.Parent.RemoveComponent<MailBoxComponent>();
        }
    }

    public class PlayerCall : Entity, IAwake<long>, IDestroy
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