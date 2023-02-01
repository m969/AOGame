namespace AO
{
    using ET;
    using ActorSendEvent = AO.EventType.ActorSendEvent;

    public class AvatarCall : Entity, IAwake
    {
        public ClientCall Client { get; private set; } = new ClientCall();

        public class ClientCall : Entity, IAwake
        {
			public void M2C_CreateUnits(M2C_CreateUnits msg) => AOGame.Publish(new ActorSendEvent() {ActorId = Id,Message = msg});
			public void M2C_CreateMyUnit(M2C_CreateMyUnit msg) => AOGame.Publish(new ActorSendEvent() {ActorId = Id,Message = msg});
			public void M2C_StartSceneChange(M2C_StartSceneChange msg) => AOGame.Publish(new ActorSendEvent() {ActorId = Id,Message = msg});
			public void M2C_RemoveUnits(M2C_RemoveUnits msg) => AOGame.Publish(new ActorSendEvent() {ActorId = Id,Message = msg});
			public void M2C_PathfindingResult(M2C_PathfindingResult msg) => AOGame.Publish(new ActorSendEvent() {ActorId = Id,Message = msg});
			public void M2C_Stop(M2C_Stop msg) => AOGame.Publish(new ActorSendEvent() {ActorId = Id,Message = msg});

        }

        public class InnerCall : Entity, IAwake
        {

        }
    }
}