namespace AO
{
    using AO;
    using EGamePlay;
    using ET;
    using ET.Server;
    using System.Collections.Generic;
    using Unity.Mathematics;

    public static partial class PlayerOuterRequests
    {
        public static partial async ETTask C2G_EnterMap(Player player, C2G_EnterMap request, G2C_EnterMap response)
        {
            response.MyId = player.Id;
            if (player.UnitId == 0)
            {
                player.CheckAvatarEnterMap().Coroutine();
            }
            await ETTask.CompletedTask;
        }
    }
}