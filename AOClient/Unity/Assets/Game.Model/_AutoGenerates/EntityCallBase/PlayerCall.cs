namespace AO
{
    using ET;
    using System.Threading.Tasks;

    public static class PlayerCall
    {
        public static async ETTask<G2C_EnterMap> C2G_EnterMap(C2G_EnterMap request)
        {
            return await new EventType.RequestCall().CallAsync(request) as G2C_EnterMap;
        }


    }

    public static class PlayerCallTs
    {
        public static async Task<G2C_EnterMap> C2G_EnterMap(C2G_EnterMap request)
        {
            return await new EventType.RequestCall().CallAsync(request) as G2C_EnterMap;
        }


    }
}