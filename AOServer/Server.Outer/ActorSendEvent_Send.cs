namespace AO
{
    using ET;
    using AO;
    using System.Net;
    using ET.Server;

    [Event(SceneType.Process)]
    public class ActorSendEvent_Send : AEvent<EventType.ActorSendEvent>
    {
        protected override async ETTask Run(Scene scene, EventType.ActorSendEvent args)
        {
            ActorMessageSenderComponent.Instance?.Send(args.ActorId, args.Message);
            await ETTask.CompletedTask;
        }
    }
}