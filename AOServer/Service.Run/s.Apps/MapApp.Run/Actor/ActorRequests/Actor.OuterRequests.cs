namespace AO
{
    using AO;
    using ET;
    using EGamePlay;
    using EGamePlay.Combat;

    public static partial class ActorOuterRequests
    {
        public static partial async ETTask C2M_PathfindingResult(Actor avatar, C2M_PathfindingResult request)
        {
            avatar.MoveToAsync(request.Position).Coroutine();
            await ETTask.CompletedTask;
        }

        public static partial async ETTask C2M_Stop(Actor avatar, C2M_Stop request)
        {
            await ETTask.CompletedTask;
        }

        public static async partial ETTask C2M_TestRobotCase(Actor avatar, C2M_TestRobotCase request, M2C_TestRobotCase response)
        {
            await ETTask.CompletedTask;
        }

        public static async partial ETTask Actor_TransferRequest(Actor avatar, Actor_TransferRequest request, Actor_TransferResponse response)
        {
            await ETTask.CompletedTask;
        }

        public static async partial ETTask C2M_TestRequest(Actor avatar, C2M_TestRequest request, M2C_TestResponse response)
        {
            await ETTask.CompletedTask;
        }

        public static async partial ETTask C2M_TransferMap(Actor avatar, C2M_TransferMap request, M2C_TransferMap response)
        {
            await ETTask.CompletedTask;
        }
    }
}