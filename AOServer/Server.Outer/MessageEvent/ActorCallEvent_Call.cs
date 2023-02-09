namespace AO
{
    using ET;
    using AO;
    using System.Net;
    using ET.Server;

    [Event(SceneType.Process)]
    public class ActorCallEvent_Call : AEvent<EventType.ActorCallEvent>
    {
        protected override async ETTask Run(Entity source, EventType.ActorCallEvent args)
        {
            var response = await ActorMessageSenderComponent.Instance?.Call(args.ActorId, args.Message);
            args.Task.SetResult(response);
        }
    }
}