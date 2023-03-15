namespace AO
{
    using ET;
    using ET.Server;
    using ActorSendEvent = AO.EventType.ActorSendEvent;
    using BroadcastEvent = AO.EventType.BroadcastEvent;

    public class AvatarClientAwakeSystem: AwakeSystem<AvatarClient, long>
    {
        protected override void Awake(AvatarClient self, long sessionId)
        {
            self.Parent.AddComponent<GateSessionIdComponent, long>(sessionId);
            self.Parent.AddComponent<MailBoxComponent>();
            self.Client = new AvatarClient.ClientCall();
            self.Client.SessionId = sessionId;
            self.AOIClients = new AvatarClient.AOICall();
            self.AOIClients.Unit = self.Parent as IMapUnit;
        }
    }

    public class AvatarClientDestroyHandler : DestroySystem<AvatarClient>
    {
        protected override void Destroy(AvatarClient self)
        {
            self.Parent.RemoveComponent<GateSessionIdComponent>();
            self.Parent.RemoveComponent<MailBoxComponent>();
        }
    }

    public class AvatarClient : Entity, IAwake<long>, IDestroy
    {
        public ClientCall Client { get; set; }
        public AOICall AOIClients { get; set; }

        public class ClientCall
        {
            public long SessionId { get; set; }
			public void M2C_OnEnterMap(M2C_OnEnterMap msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
			public void M2C_OnLeaveMap(M2C_OnLeaveMap msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
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
//ResponseType M2C_SpellResponse
			public void M2C_SpellStart(M2C_SpellStart msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
			public void M2C_SpellStep(M2C_SpellStep msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });
			public void M2C_SpellEnd(M2C_SpellEnd msg) => AOGame.Publish(new ActorSendEvent() { ActorId = SessionId, Message = msg });

        }
        public class AOICall
        {
            public IMapUnit Unit { get; set; }
			public void M2C_OnEnterMap(M2C_OnEnterMap msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });
			public void M2C_OnLeaveMap(M2C_OnLeaveMap msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });
			public void M2C_CreateUnits(M2C_CreateUnits msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });
			public void M2C_CreateMyUnit(M2C_CreateMyUnit msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });
			public void M2C_RemoveUnits(M2C_RemoveUnits msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });
			public void M2C_ComponentPropertyNotify(M2C_ComponentPropertyNotify msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });
			public void M2C_StartSceneChange(M2C_StartSceneChange msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });
			public void M2C_PathfindingResult(M2C_PathfindingResult msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });
			public void M2C_Stop(M2C_Stop msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });
//ResponseType M2C_TestRobotCase
//ResponseType M2C_TestResponse
//ResponseType Actor_TransferResponse
//ResponseType M2C_TransferMap
//ResponseType M2C_SpellResponse
			public void M2C_SpellStart(M2C_SpellStart msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });
			public void M2C_SpellStep(M2C_SpellStep msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });
			public void M2C_SpellEnd(M2C_SpellEnd msg) => AOGame.Publish(new BroadcastEvent() { Unit = Unit, Message = msg });

        }
    }
}