namespace AO
{
    using ET;
    using ET.Server;
    using ActorCallEvent = AO.EventType.ActorCallEvent;
    using BroadcastEvent = AO.EventType.BroadcastEvent;

    public class SceneCallAwakeSystem: AwakeSystem<SceneCall, long>
    {
        protected override void Awake(SceneCall self, long actorId)
        {
            self.EntityActorId = actorId;
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