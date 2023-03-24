namespace AO
{
    using ET;
    using ET.Server;
    using ActorCallEvent = AO.EventType.ActorCallEvent;
    using BroadcastEvent = AO.EventType.BroadcastEvent;

    public class DBCacheAppCallAwakeSystem: AwakeSystem<DBCacheAppCall, long>
    {
        protected override void Awake(DBCacheAppCall self, long actorId)
        {
            self.EntityActorId = actorId;
        }
    }

    public class DBCacheAppCall : Entity, IAwake<long>, IDestroy
    {
        public long EntityActorId { get; set; }

        public async ETTask<DB2A_Query> A2DB_Query(A2DB_Query msg)
        {
            var msgCall = new ActorCallEvent() { ActorId = EntityActorId, Message = msg, Task = ETTask<IActorResponse>.Create() };
            AOGame.Publish(msgCall);
            var response = await msgCall.Task;
            return response as DB2A_Query;
        }

        public async ETTask<DBCacheSaveResponse> DBCacheSaveRequest(DBCacheSaveRequest msg)
        {
            var msgCall = new ActorCallEvent() { ActorId = EntityActorId, Message = msg, Task = ETTask<IActorResponse>.Create() };
            AOGame.Publish(msgCall);
            var response = await msgCall.Task;
            return response as DBCacheSaveResponse;
        }


    }
}