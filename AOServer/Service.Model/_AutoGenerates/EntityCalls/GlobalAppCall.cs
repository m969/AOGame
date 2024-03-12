namespace AO
{
    using ET;
    using ET.Server;
    using ActorCallEvent = AO.EventType.ActorCallEvent;
    using BroadcastEvent = AO.EventType.BroadcastEvent;

    public class GlobalAppCallAwakeSystem: AwakeSystem<GlobalAppCall, long>
    {
        protected override void Awake(GlobalAppCall self, long actorId)
        {
            self.EntityActorId = actorId;
        }
    }

    public class GlobalAppCall : Entity, IAwake<long>, IDestroy
    {
        public long EntityActorId { get; set; }

        public async ETTask<GetAppResponse> GetAppRequest(GetAppRequest msg)
        {
            var msgCall = new ActorCallEvent() { ActorId = EntityActorId, Message = msg, Task = ETTask<IActorResponse>.Create() };
            AOGame.Publish(msgCall);
            var response = await msgCall.Task;
            return response as GetAppResponse;
        }


    }
}