namespace AO
{
    using ET;
    using System.Threading.Tasks;

    public static class PlayerCall
    {
        public static async Task<G2C_EnterMap> C2G_EnterMap(C2G_EnterMap request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as G2C_EnterMap;
        }


    }
}