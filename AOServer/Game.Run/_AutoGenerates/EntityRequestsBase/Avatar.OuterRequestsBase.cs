namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static partial class AvatarOuterRequests
    {
        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_TestRobotCaseHandler : AMActorLocationRpcHandler<Avatar, C2M_TestRobotCase, M2C_TestRobotCase>
        {
            protected override async ETTask Run(Avatar entity, C2M_TestRobotCase request, M2C_TestRobotCase response)
            {
                await AvatarOuterRequests.C2M_TestRobotCase(entity, request, response);
            }
        }
		public static partial ETTask C2M_TestRobotCase(Avatar avatar, C2M_TestRobotCase request, M2C_TestRobotCase response);

        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_TestRequestHandler : AMActorLocationRpcHandler<Avatar, C2M_TestRequest, M2C_TestResponse>
        {
            protected override async ETTask Run(Avatar entity, C2M_TestRequest request, M2C_TestResponse response)
            {
                await AvatarOuterRequests.C2M_TestRequest(entity, request, response);
            }
        }
		public static partial ETTask C2M_TestRequest(Avatar avatar, C2M_TestRequest request, M2C_TestResponse response);

        [ActorMessageHandler(SceneType.Gate)]
        public class Actor_TransferRequestHandler : AMActorLocationRpcHandler<Avatar, Actor_TransferRequest, Actor_TransferResponse>
        {
            protected override async ETTask Run(Avatar entity, Actor_TransferRequest request, Actor_TransferResponse response)
            {
                await AvatarOuterRequests.Actor_TransferRequest(entity, request, response);
            }
        }
		public static partial ETTask Actor_TransferRequest(Avatar avatar, Actor_TransferRequest request, Actor_TransferResponse response);

        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_TransferMapHandler : AMActorLocationRpcHandler<Avatar, C2M_TransferMap, M2C_TransferMap>
        {
            protected override async ETTask Run(Avatar entity, C2M_TransferMap request, M2C_TransferMap response)
            {
                await AvatarOuterRequests.C2M_TransferMap(entity, request, response);
            }
        }
		public static partial ETTask C2M_TransferMap(Avatar avatar, C2M_TransferMap request, M2C_TransferMap response);

        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_SpellCastRequestHandler : AMActorLocationRpcHandler<Avatar, C2M_SpellCastRequest, M2C_SpellCastResponse>
        {
            protected override async ETTask Run(Avatar entity, C2M_SpellCastRequest request, M2C_SpellCastResponse response)
            {
                await AvatarOuterRequests.C2M_SpellCastRequest(entity, request, response);
            }
        }
		public static partial ETTask C2M_SpellCastRequest(Avatar avatar, C2M_SpellCastRequest request, M2C_SpellCastResponse response);


    }
}