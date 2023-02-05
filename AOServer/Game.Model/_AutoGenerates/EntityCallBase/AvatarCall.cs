namespace AO
{
    using ET;
    using ET.Server;
    using ActorSendEvent = AO.EventType.ActorSendEvent;

    public class AvatarCallAwakeSystem: AwakeSystem<AvatarCall, long>
    {
        protected override void Awake(AvatarCall self, long sessionId)
        {
            self.Parent.AddComponent<GateSessionIdComponent, long>(sessionId);
            self.Parent.AddComponent<MailBoxComponent>();
            self.Client = new AvatarCall.ClientCall();
            self.Client.SessionId = sessionId;
        }
    }

    public class AvatarCall : Entity, IAwake<long>
    {
        public ClientCall Client { get; set; }

        public class ClientCall
        {
            public long SessionId { get; set; }
			public void M2C_CreateUnits(M2C_CreateUnits msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
			public void M2C_CreateMyUnit(M2C_CreateMyUnit msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
			public void M2C_RemoveUnits(M2C_RemoveUnits msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
			public void M2C_ComponentPropertyNotify(M2C_ComponentPropertyNotify msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
			public void M2C_StartSceneChange(M2C_StartSceneChange msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
			public void M2C_PathfindingResult(M2C_PathfindingResult msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
			public void M2C_Stop(M2C_Stop msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
//ResponseType M2C_TestRobotCase
//ResponseType M2C_TestResponse
//ResponseType Actor_TransferResponse
//ResponseType M2C_TransferMap
//ResponseType M2C_SpellCastResponse
			public void M2C_SpellCastStart(M2C_SpellCastStart msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
			public void M2C_SpellCastEnd(M2C_SpellCastEnd msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });

        }
    }
}