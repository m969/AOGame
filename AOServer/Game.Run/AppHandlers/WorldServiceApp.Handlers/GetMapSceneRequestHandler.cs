using AO;
using System;


namespace ET.Server
{
    [ActorMessageHandler(SceneType.Gate)]
	public class GetMapSceneRequestHandler : AMActorLocationRpcHandler<WorldServiceApp, GetMapSceneRequest, GetMapSceneResponse>
	{
		protected override async ETTask Run(WorldServiceApp session, GetMapSceneRequest request, GetMapSceneResponse response)
		{
			Log.Console("GetMapSceneRequestHandler");
			await ETTask.CompletedTask;
		}
	}
}