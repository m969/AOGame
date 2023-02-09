namespace AO
{
    using ET;
    using AO;
    using System.Net;
    using ET.Server;

    [Event(SceneType.Process)]
    public class BroadcastEvent_Send : AEvent<EventType.BroadcastEvent>
    {
        protected override async ETTask Run(Entity source, EventType.BroadcastEvent args)
        {
            var unit = args.Unit as Entity;
            var unitScene = unit.GetParent<Scene>();
            var sceneAvatars = unitScene.GetComponent<SceneUnitComponent>().idAvatars;
            foreach (var item in sceneAvatars)
            {
                ActorMessageSenderComponent.Instance?.Send(item.Value.GetComponent<GateSessionIdComponent>().GateSessionId, args.Message);
            }
            await ETTask.CompletedTask;
        }
    }
}