using AO;
using System;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Location)]
    public class ObjectGetRequestHandler: AMActorRpcHandler<ActorIdApp, ObjectGetRequest, ObjectGetResponse>
    {
        protected override async ETTask Run(ActorIdApp scene, ObjectGetRequest request, ObjectGetResponse response)
        {
            long instanceId = await scene.GetComponent<LocationComponent>().Get(request.Key);
            response.InstanceId = instanceId;
        }
    }
}