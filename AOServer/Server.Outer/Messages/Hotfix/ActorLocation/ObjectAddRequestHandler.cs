using AO;
using System;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Location)]
    public class ObjectAddRequestHandler: AMActorRpcHandler<ActorIdApp, ObjectAddRequest, ObjectAddResponse>
    {
        protected override async ETTask Run(ActorIdApp scene, ObjectAddRequest request, ObjectAddResponse response)
        {
            await scene.GetComponent<LocationComponent>().Add(request.Key, request.InstanceId);
        }
    }
}