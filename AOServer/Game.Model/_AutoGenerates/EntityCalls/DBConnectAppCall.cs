namespace AO
{
    using ET;
    using ET.Server;
    using ActorCallEvent = AO.EventType.ActorCallEvent;
    using BroadcastEvent = AO.EventType.BroadcastEvent;

    public class DBConnectAppCallAwakeSystem: AwakeSystem<DBConnectAppCall, long>
    {
        protected override void Awake(DBConnectAppCall self, long actorId)
        {
            self.EntityActorId = actorId;
        }
    }

    public class DBConnectAppCall : Entity, IAwake<long>, IDestroy
    {
        public long EntityActorId { get; set; }


    }
}