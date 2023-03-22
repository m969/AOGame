namespace AO
{
    using ET;
    using ET.Server;
    using ActorCallEvent = AO.EventType.ActorCallEvent;
    using BroadcastEvent = AO.EventType.BroadcastEvent;

    public class MapAppCallAwakeSystem: AwakeSystem<MapAppCall, long>
    {
        protected override void Awake(MapAppCall self, long actorId)
        {
            self.EntityActorId = actorId;
        }
    }

    public class MapAppCall : Entity, IAwake<long>, IDestroy
    {
        public long EntityActorId { get; set; }


    }
}