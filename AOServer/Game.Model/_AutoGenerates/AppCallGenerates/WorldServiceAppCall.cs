namespace AO
{
    using AO.EventType;
    using ET;
    using ET.Server;
    using System;
    using ActorSendEvent = AO.EventType.ActorSendEvent;
    using BroadcastEvent = AO.EventType.BroadcastEvent;

    public class WorldServiceAppCallAwakeSystem : AwakeSystem<WorldServiceAppCall, long>
    {
        protected override void Awake(WorldServiceAppCall self, long appId)
        {
            self.AppId = appId;
        }
    }

    public class WorldServiceAppCall : Entity, IAwake<long>, IDestroy
    {
        public long AppId { get; set; }

        public async ETTask<GetMapSceneResponse> GetMapSceneRequest(GetMapSceneRequest msg)
        {
            var msgCall = new ActorCallEvent() { ActorId = AppId, Message = msg, Task = ETTask<IActorResponse>.Create() };
            AOGame.Publish(msgCall);
            var response = await msgCall.Task;
            return response as GetMapSceneResponse;
        }
    }
}