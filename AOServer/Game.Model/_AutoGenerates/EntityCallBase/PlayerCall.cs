namespace AO
{
    using ET;
    using ET.Server;
    using ActorSendEvent = AO.EventType.ActorSendEvent;

    public class PlayerCallAwakeSystem: AwakeSystem<PlayerCall, long>
    {
        protected override void Awake(PlayerCall self, long sessionId)
        {
            self.Parent.AddComponent<GateSessionIdComponent, long>(sessionId);
            self.Parent.AddComponent<MailBoxComponent>();
            self.Client = new PlayerCall.ClientCall();
            self.Client.SessionId = sessionId;
        }
    }

    public class PlayerCall : Entity, IAwake<long>
    {
        public ClientCall Client { get; set; }

        public class ClientCall
        {
            public long SessionId { get; set; }

        }
    }
}