namespace AO
{
    using ET;
    using AO;
    using System.Net;
    using ET.Server;

    [Event(SceneType.Process)]
    public class ActorLocationCallEvent_Call : AEvent<EventType.ActorLocationCallEvent>
    {
        protected override async ETTask Run(Entity source, EventType.ActorLocationCallEvent args)
        {
            var response = await ActorLocationSenderComponent.Instance?.Call(args.EntityId, args.Message);
            args.Task.SetResult(response);
        }
    }
}