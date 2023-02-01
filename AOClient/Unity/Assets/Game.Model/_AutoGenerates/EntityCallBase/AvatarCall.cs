namespace AO
{
    using ET;
    using System.Threading.Tasks;

    public static class AvatarCall
    {
        public static async Task<M2C_TestRobotCase> C2M_TestRobotCase(C2M_TestRobotCase request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as M2C_TestRobotCase;
        }

        public static async Task<M2C_TestResponse> C2M_TestRequest(C2M_TestRequest request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as M2C_TestResponse;
        }

        public static async Task<Actor_TransferResponse> Actor_TransferRequest(Actor_TransferRequest request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as Actor_TransferResponse;
        }

        public static async Task<M2C_TransferMap> C2M_TransferMap(C2M_TransferMap request)
        {
            var msg = new EventType.RequestCall();
            await msg.CallAsync(request);
            return msg.Response as M2C_TransferMap;
        }


    }
}