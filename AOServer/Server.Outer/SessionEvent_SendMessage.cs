namespace AO
{
    using ET;
    using AO;
    using System.Net;
    using ET.Server;

    [Event(SceneType.Process)]
    public class SessionEvent_SendMessage : AEvent<EventType.SessionEvent>
    {
        protected override async ETTask Run(Scene scene, EventType.SessionEvent args)
        {
            var session = args.Session as Session;
            session.Send(args.Message);
            await ETTask.CompletedTask;
        }
    }
}