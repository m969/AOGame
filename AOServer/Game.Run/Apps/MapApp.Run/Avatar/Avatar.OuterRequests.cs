namespace AO
{
    using AO;
    using ET;
    using EGamePlay;
    using EGamePlay.Combat;

    public static partial class AvatarOuterRequests
    {
        public static partial async ETTask C2M_PathfindingResult(Avatar avatar, C2M_PathfindingResult request)
        {
            avatar.MoveToAsync(request.Position).Coroutine();
            await ETTask.CompletedTask;
        }

        public static partial async ETTask C2M_Stop(Avatar avatar, C2M_Stop request)
        {
            await ETTask.CompletedTask;
        }

        public static async partial ETTask C2M_TestRobotCase(Avatar avatar, C2M_TestRobotCase request, M2C_TestRobotCase response)
        {
            await ETTask.CompletedTask;
        }

        public static async partial ETTask Actor_TransferRequest(Avatar avatar, Actor_TransferRequest request, Actor_TransferResponse response)
        {
            await ETTask.CompletedTask;
        }

        public static async partial ETTask C2M_TestRequest(Avatar avatar, C2M_TestRequest request, M2C_TestResponse response)
        {
            await ETTask.CompletedTask;
        }

        public static async partial ETTask C2M_TransferMap(Avatar avatar, C2M_TransferMap request, M2C_TransferMap response)
        {
            await ETTask.CompletedTask;
        }
    }
}