namespace AO
{
    using ET;
    using AO;
    using System.Net;
    using ET.Server;

    [Event(SceneType.Process)]
    public class ActorLocationSendEvent_Send : AEvent<EventType.ActorLocationSendEvent>
    {
        protected override async ETTask Run(Entity source, EventType.ActorLocationSendEvent args)
        {
            ActorLocationSenderComponent.Instance?.Send(args.EntityId, args.Message);
            await ETTask.CompletedTask;
        }
    }
}