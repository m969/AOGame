namespace AO
{
    using ET;
    using System.Threading.Tasks;

    public static class ServerCall
    {
        public static async ETTask<G2C_Ping> C2G_Ping(C2G_Ping request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as G2C_Ping;
        }

        public static async ETTask<M2C_Reload> C2M_Reload(C2M_Reload request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as M2C_Reload;
        }

        public static async ETTask<R2C_Login> C2R_Login(C2R_Login request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as R2C_Login;
        }

        public static async ETTask<G2C_LoginGate> C2G_LoginGate(C2G_LoginGate request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as G2C_LoginGate;
        }

        public static async ETTask<G2C_Benchmark> C2G_Benchmark(C2G_Benchmark request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as G2C_Benchmark;
        }


    }

    public static class ServerCallTs
    {
        public static async Task<G2C_Ping> C2G_Ping(C2G_Ping request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as G2C_Ping;
        }

        public static async Task<M2C_Reload> C2M_Reload(C2M_Reload request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as M2C_Reload;
        }

        public static async Task<R2C_Login> C2R_Login(C2R_Login request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as R2C_Login;
        }

        public static async Task<G2C_LoginGate> C2G_LoginGate(C2G_LoginGate request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as G2C_LoginGate;
        }

        public static async Task<G2C_Benchmark> C2G_Benchmark(C2G_Benchmark request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as G2C_Benchmark;
        }


    }
}