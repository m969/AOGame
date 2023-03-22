namespace AO
{
    using ET;
    using AO;
    using System.Net;
    using ET.Server;

    [Event(SceneType.Process)]
    public class SessionRemoveAcceptTimeoutComponentEvent_Handler : AEvent<EventType.SessionRemoveAcceptTimeoutComponentEvent>
    {
        protected override async ETTask Run(Entity source, EventType.SessionRemoveAcceptTimeoutComponentEvent args)
        {
            source.RemoveComponent<SessionAcceptTimeoutComponent>();
            await ETTask.CompletedTask;
        }
    }
}