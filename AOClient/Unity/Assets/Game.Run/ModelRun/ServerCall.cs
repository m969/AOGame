namespace AO
{
    using ET;
    using System;
    using System.Threading.Tasks;

    public static class ServerCall
    {
        public static async Task<G2C_LoginGate> C2G_LoginGate(C2G_LoginGate message)
        {
            var msgCall = new EventType.RequestCall();
            await msgCall.CallAsync(message);
            return msgCall.Response as G2C_LoginGate;
        }

        public static async Task<G2C_EnterMap> C2G_EnterMap(C2G_EnterMap message)
        {
            var msgCall = new EventType.RequestCall();
            await msgCall.CallAsync(message);
            return msgCall.Response as G2C_EnterMap;
        }
    }
}