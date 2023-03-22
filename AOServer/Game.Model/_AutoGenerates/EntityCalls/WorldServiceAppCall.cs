namespace AO
{
    using ET;
    using ET.Server;
    using ActorCallEvent = AO.EventType.ActorCallEvent;
    using BroadcastEvent = AO.EventType.BroadcastEvent;

    public class WorldServiceAppCallAwakeSystem: AwakeSystem<WorldServiceAppCall, long>
    {
        protected override void Awake(WorldServiceAppCall self, long actorId)
        {
            self.EntityActorId = actorId;
        }
    }

    public class WorldServiceAppCall : Entity, IAwake<long>, IDestroy
    {
        public long EntityActorId { get; set; }

        public async ETTask<ActorResponse> RegisterMapSceneRequest(RegisterMapSceneRequest msg)
        {
            var msgCall = new ActorCallEvent() { ActorId = EntityActorId, Message = msg, Task = ETTask<IActorResponse>.Create() };
            AOGame.Publish(msgCall);
            var response = await msgCall.Task;
            return response as ActorResponse;
        }

        public async ETTask<GetMapSceneResponse> GetMapSceneRequest(GetMapSceneRequest msg)
        {
            var msgCall = new ActorCallEvent() { ActorId = EntityActorId, Message = msg, Task = ETTask<IActorResponse>.Create() };
            AOGame.Publish(msgCall);
            var response = await msgCall.Task;
            return response as GetMapSceneResponse;
        }

        public async ETTask<EnterMapResponse> EnterMapRequest(EnterMapRequest msg)
        {
            var msgCall = new ActorCallEvent() { ActorId = EntityActorId, Message = msg, Task = ETTask<IActorResponse>.Create() };
            AOGame.Publish(msgCall);
            var response = await msgCall.Task;
            return response as EnterMapResponse;
        }


    }
}