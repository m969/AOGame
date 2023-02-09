namespace AO
{
    using ET;
    using AO;
    using System.Net;
    using ET.Server;

    [Event(SceneType.Process)]
    public class ActorEvent_Reply : AEvent<EventType.ActorReplyEvent>
    {
        protected override async ETTask Run(Entity source, EventType.ActorReplyEvent args)
        {
            ActorHandleHelper.Reply(args.FromProcess, args.Message);
            await ETTask.CompletedTask;
        }
    }
}