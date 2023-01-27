using AO;
using System;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Location)]
    public class ObjectRemoveRequestHandler: AMActorRpcHandler<ActorIdApp, ObjectRemoveRequest, ObjectRemoveResponse>
    {
        protected override async ETTask Run(ActorIdApp scene, ObjectRemoveRequest request, ObjectRemoveResponse response)
        {
            await scene.GetComponent<LocationComponent>().Remove(request.Key);
        }
    }
}