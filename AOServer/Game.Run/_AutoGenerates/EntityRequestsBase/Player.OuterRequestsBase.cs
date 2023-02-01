namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static partial class PlayerOuterRequests
    {
        [ActorMessageHandler(SceneType.Gate)]
        public class C2G_EnterMapHandler : AMActorLocationRpcHandler<Player, C2G_EnterMap, G2C_EnterMap>
        {
            protected override async ETTask Run(Player entity, C2G_EnterMap request, G2C_EnterMap response)
            {
                await PlayerOuterRequests.C2G_EnterMap(entity, request, response);
            }
        }
		public static partial ETTask C2G_EnterMap(Player player, C2G_EnterMap request, G2C_EnterMap response);


    }
}