using AO;
using System;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Location)]
    public class ObjectLockRequestHandler: AMActorRpcHandler<ActorIdApp, ObjectLockRequest, ObjectLockResponse>
    {
        protected override async ETTask Run(ActorIdApp scene, ObjectLockRequest request, ObjectLockResponse response)
        {
            await scene.GetComponent<LocationComponent>().Lock(request.Key, request.InstanceId, request.Time);
        }
    }
}