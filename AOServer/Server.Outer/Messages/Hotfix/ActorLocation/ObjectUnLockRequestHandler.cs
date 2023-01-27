using AO;
using System;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Location)]
    public class ObjectUnLockRequestHandler: AMActorRpcHandler<ActorIdApp, ObjectUnLockRequest, ObjectUnLockResponse>
    {
        protected override async ETTask Run(ActorIdApp scene, ObjectUnLockRequest request, ObjectUnLockResponse response)
        {
            scene.GetComponent<LocationComponent>().UnLock(request.Key, request.OldInstanceId, request.InstanceId);

            await ETTask.CompletedTask;
        }
    }
}