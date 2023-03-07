namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static partial class AvatarInnerRequests
    {
        [ActorMessageHandler(SceneType.Gate)]
        public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Avatar, G2M_SessionDisconnect>
        {
            protected override async ETTask Run(Avatar entity, G2M_SessionDisconnect request)
            {
                await G2M_SessionDisconnect(entity, request);
            }
        }

        public static async ETTask G2M_SessionDisconnect(Avatar avatar, G2M_SessionDisconnect request)
        {
            Log.Console($"G2M_SessionDisconnect {avatar.Id}");
            avatar.GetParent<Scene>().GetComponent<SceneUnitComponent>().Remove(avatar.Id);
            avatar.RemoveComponent<AvatarCall>();
            avatar.Dispose();
            await ETTask.CompletedTask;
        }
    }
}