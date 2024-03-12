namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static partial class ActorInnerRequests
    {
        [ActorMessageHandler(SceneType.Gate)]
        public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Actor, G2M_SessionDisconnect>
        {
            protected override async ETTask Run(Actor entity, G2M_SessionDisconnect request)
            {
                await G2M_SessionDisconnect(entity, request);
            }
        }

        public static async ETTask G2M_SessionDisconnect(Actor avatar, G2M_SessionDisconnect request)
        {
            Log.Console($"G2M_SessionDisconnect {avatar.Id}");
            avatar.GetParent<Scene>().GetComponent<SceneUnitComponent>().Remove(avatar.Id);
            avatar.RemoveComponent<ActorClient>();
            avatar.Dispose();
            await ETTask.CompletedTask;
        }
    }
}