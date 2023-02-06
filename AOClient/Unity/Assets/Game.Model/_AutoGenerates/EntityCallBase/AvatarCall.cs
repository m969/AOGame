namespace AO
{
    using ET;
    using System.Threading.Tasks;

    public static class AvatarCall
    {
        public static void C2M_PathfindingResult(C2M_PathfindingResult request) => EventType.RequestCall.SendAction(request);

        public static void C2M_Stop(C2M_Stop request) => EventType.RequestCall.SendAction(request);

        public static async ETTask<M2C_TestRobotCase> C2M_TestRobotCase(C2M_TestRobotCase request)
        {
            return await new EventType.RequestCall().CallAsync(request) as M2C_TestRobotCase;
        }

        public static async ETTask<M2C_TestResponse> C2M_TestRequest(C2M_TestRequest request)
        {
            return await new EventType.RequestCall().CallAsync(request) as M2C_TestResponse;
        }

        public static async ETTask<Actor_TransferResponse> Actor_TransferRequest(Actor_TransferRequest request)
        {
            return await new EventType.RequestCall().CallAsync(request) as Actor_TransferResponse;
        }

        public static async ETTask<M2C_TransferMap> C2M_TransferMap(C2M_TransferMap request)
        {
            return await new EventType.RequestCall().CallAsync(request) as M2C_TransferMap;
        }

        public static async ETTask<M2C_SpellCastResponse> C2M_SpellCastRequest(C2M_SpellCastRequest request)
        {
            return await new EventType.RequestCall().CallAsync(request) as M2C_SpellCastResponse;
        }


    }

    public static class AvatarCallTs
    {
        public static async Task<M2C_TestRobotCase> C2M_TestRobotCase(C2M_TestRobotCase request)
        {
            return await new EventType.RequestCall().CallAsync(request) as M2C_TestRobotCase;
        }

        public static async Task<M2C_TestResponse> C2M_TestRequest(C2M_TestRequest request)
        {
            return await new EventType.RequestCall().CallAsync(request) as M2C_TestResponse;
        }

        public static async Task<Actor_TransferResponse> Actor_TransferRequest(Actor_TransferRequest request)
        {
            return await new EventType.RequestCall().CallAsync(request) as Actor_TransferResponse;
        }

        public static async Task<M2C_TransferMap> C2M_TransferMap(C2M_TransferMap request)
        {
            return await new EventType.RequestCall().CallAsync(request) as M2C_TransferMap;
        }

        public static async Task<M2C_SpellCastResponse> C2M_SpellCastRequest(C2M_SpellCastRequest request)
        {
            return await new EventType.RequestCall().CallAsync(request) as M2C_SpellCastResponse;
        }


    }
}