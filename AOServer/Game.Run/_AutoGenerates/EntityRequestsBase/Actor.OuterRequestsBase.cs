namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static partial class ActorOuterRequests
    {
        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_PathfindingResultHandler : AMActorLocationHandler<Actor, C2M_PathfindingResult>
        {
            protected override async ETTask Run(Actor entity, C2M_PathfindingResult request)
            {
                await C2M_PathfindingResult(entity, request);
            }
        }

        public static partial ETTask C2M_PathfindingResult(Actor actor, C2M_PathfindingResult request);


        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_StopHandler : AMActorLocationHandler<Actor, C2M_Stop>
        {
            protected override async ETTask Run(Actor entity, C2M_Stop request)
            {
                await C2M_Stop(entity, request);
            }
        }

        public static partial ETTask C2M_Stop(Actor actor, C2M_Stop request);


        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_TestRobotCaseHandler : AMActorLocationRpcHandler<Actor, C2M_TestRobotCase, M2C_TestRobotCase>
        {
            protected override async ETTask Run(Actor entity, C2M_TestRobotCase request, M2C_TestRobotCase response)
            {
                await C2M_TestRobotCase(entity, request, response);
            }
        }

        public static partial ETTask C2M_TestRobotCase(Actor actor, C2M_TestRobotCase request, M2C_TestRobotCase response);


        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_TestRequestHandler : AMActorLocationRpcHandler<Actor, C2M_TestRequest, M2C_TestResponse>
        {
            protected override async ETTask Run(Actor entity, C2M_TestRequest request, M2C_TestResponse response)
            {
                await C2M_TestRequest(entity, request, response);
            }
        }

        public static partial ETTask C2M_TestRequest(Actor actor, C2M_TestRequest request, M2C_TestResponse response);


        [ActorMessageHandler(SceneType.Gate)]
        public class Actor_TransferRequestHandler : AMActorLocationRpcHandler<Actor, Actor_TransferRequest, Actor_TransferResponse>
        {
            protected override async ETTask Run(Actor entity, Actor_TransferRequest request, Actor_TransferResponse response)
            {
                await Actor_TransferRequest(entity, request, response);
            }
        }

        public static partial ETTask Actor_TransferRequest(Actor actor, Actor_TransferRequest request, Actor_TransferResponse response);


        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_TransferMapHandler : AMActorLocationRpcHandler<Actor, C2M_TransferMap, M2C_TransferMap>
        {
            protected override async ETTask Run(Actor entity, C2M_TransferMap request, M2C_TransferMap response)
            {
                await C2M_TransferMap(entity, request, response);
            }
        }

        public static partial ETTask C2M_TransferMap(Actor actor, C2M_TransferMap request, M2C_TransferMap response);


        [ActorMessageHandler(SceneType.Gate)]
        public class C2M_SpellRequestHandler : AMActorLocationRpcHandler<Actor, C2M_SpellRequest, M2C_SpellResponse>
        {
            protected override async ETTask Run(Actor entity, C2M_SpellRequest request, M2C_SpellResponse response)
            {
                await C2M_SpellRequest(entity, request, response);
            }
        }

        public static partial ETTask C2M_SpellRequest(Actor actor, C2M_SpellRequest request, M2C_SpellResponse response);



    }
}