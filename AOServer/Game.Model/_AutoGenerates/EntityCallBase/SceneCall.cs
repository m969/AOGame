namespace AO
{
    using AO.EventType;
    using ET;
    using ET.Server;
    using ActorSendEvent = AO.EventType.ActorSendEvent;
    using BroadcastEvent = AO.EventType.BroadcastEvent;

    public class SceneCallAwakeSystem : AwakeSystem<SceneCall, long>
    {
        protected override void Awake(SceneCall self, long sessionId)
        {
            self.EntityActorId = sessionId;
        }
    }

    public class SceneCallDestroyHandler : DestroySystem<SceneCall>
    {
        protected override void Destroy(SceneCall self)
        {
        }
    }

    public class SceneCall : Entity, IAwake<long>, IDestroy
    {
        public long EntityActorId { get; set; }

        public async ETTask<EnterSceneResponse> EnterSceneRequest(EnterSceneRequest msg)
        {
            var msgCall = new ActorCallEvent() { ActorId = EntityActorId, Message = msg, Task = ETTask<IActorResponse>.Create() };
            AOGame.Publish(msgCall);
            var response = await msgCall.Task;
            return response as EnterSceneResponse;
        }
    }
}