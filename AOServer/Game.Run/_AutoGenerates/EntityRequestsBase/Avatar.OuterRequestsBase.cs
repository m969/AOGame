namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static partial class AvatarOuterRequests
    {
        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_PathfindingResultHandler : AMActorLocationHandler<Avatar, C2M_PathfindingResult>
        {
            protected override async ETTask Run(Avatar entity, C2M_PathfindingResult request)
            {
                await C2M_PathfindingResult(entity, request);
            }
        }

        public static partial ETTask C2M_PathfindingResult(Avatar avatar, C2M_PathfindingResult request);


        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_StopHandler : AMActorLocationHandler<Avatar, C2M_Stop>
        {
            protected override async ETTask Run(Avatar entity, C2M_Stop request)
            {
                await C2M_Stop(entity, request);
            }
        }

        public static partial ETTask C2M_Stop(Avatar avatar, C2M_Stop request);


        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_TestRobotCaseHandler : AMActorLocationRpcHandler<Avatar, C2M_TestRobotCase, M2C_TestRobotCase>
        {
            protected override async ETTask Run(Avatar entity, C2M_TestRobotCase request, M2C_TestRobotCase response)
            {
                await C2M_TestRobotCase(entity, request, response);
            }
        }

        public static partial ETTask C2M_TestRobotCase(Avatar avatar, C2M_TestRobotCase request, M2C_TestRobotCase response);


        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_TestRequestHandler : AMActorLocationRpcHandler<Avatar, C2M_TestRequest, M2C_TestResponse>
        {
            protected override async ETTask Run(Avatar entity, C2M_TestRequest request, M2C_TestResponse response)
            {
                await C2M_TestRequest(entity, request, response);
            }
        }

        public static partial ETTask C2M_TestRequest(Avatar avatar, C2M_TestRequest request, M2C_TestResponse response);


        [ActorMessageHandler(SceneType.Gate)]
        public class Actor_TransferRequestHandler : AMActorLocationRpcHandler<Avatar, Actor_TransferRequest, Actor_TransferResponse>
        {
            protected override async ETTask Run(Avatar entity, Actor_TransferRequest request, Actor_TransferResponse response)
            {
                await Actor_TransferRequest(entity, request, response);
            }
        }

        public static partial ETTask Actor_TransferRequest(Avatar avatar, Actor_TransferRequest request, Actor_TransferResponse response);


        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_TransferMapHandler : AMActorLocationRpcHandler<Avatar, C2M_TransferMap, M2C_TransferMap>
        {
            protected override async ETTask Run(Avatar entity, C2M_TransferMap request, M2C_TransferMap response)
            {
                await C2M_TransferMap(entity, request, response);
            }
        }

        public static partial ETTask C2M_TransferMap(Avatar avatar, C2M_TransferMap request, M2C_TransferMap response);


        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_SpellRequestHandler : AMActorLocationRpcHandler<Avatar, C2M_SpellRequest, M2C_SpellResponse>
        {
            protected override async ETTask Run(Avatar entity, C2M_SpellRequest request, M2C_SpellResponse response)
            {
                await C2M_SpellRequest(entity, request, response);
            }
        }

        public static partial ETTask C2M_SpellRequest(Avatar avatar, C2M_SpellRequest request, M2C_SpellResponse response);



    }
}