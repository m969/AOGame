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
            //var unitScene = unit.GetParent<Scene>();
            //var sceneAvatars = unitScene.GetComponent<SceneUnitComponent>().idAvatars;
            var aoiComp = unit.GetComponent<AOIEntity>();
            if (aoiComp == null)
            {
                return;
            }
            foreach (var item in unit.GetComponent<AOIEntity>().BeSeePlayers.Values)
            {
                ActorMessageSenderComponent.Instance?.Send(item.Unit.GetComponent<GateSessionIdComponent>().GateSessionId, args.Message);
            }
            await ETTask.CompletedTask;
        }
    }
}