namespace AO
{
    using AO;
    using ET;

    public static partial class AvatarOuterRequests
    {
        public static async partial ETTask C2M_TestRobotCase(Avatar avatar, C2M_TestRobotCase request, M2C_TestRobotCase response)
        {
        }

        public static async partial ETTask Actor_TransferRequest(Avatar avatar, Actor_TransferRequest request, Actor_TransferResponse response)
        {
        }

        public static async partial ETTask C2M_TestRequest(Avatar avatar, C2M_TestRequest request, M2C_TestResponse response)
        {
        }

        public static async partial ETTask C2M_TransferMap(Avatar avatar, C2M_TransferMap request, M2C_TransferMap response)
        {
        }

        public static async partial ETTask C2M_SpellCastRequest(Avatar avatar, C2M_SpellCastRequest request, M2C_SpellCastResponse response)
        {
            Log.Console($"C2M_SpellCastRequest {request.SkillId} {request.CastPoint} {request.CastTargetId}");
        }
    }
}