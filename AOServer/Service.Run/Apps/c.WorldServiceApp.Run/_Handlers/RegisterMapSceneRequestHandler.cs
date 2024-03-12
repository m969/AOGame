using AO;
using cfg.Map;
using EGamePlay.Combat;
using System;


namespace ET.Server
{
    [ActorMessageHandler(SceneType.Gate)]
	public class RegisterMapSceneRequestHandler : AMActorRpcHandler<WorldServiceApp, RegisterMapSceneRequest, ActorResponse>
	{
		protected override async ETTask Run(WorldServiceApp app, RegisterMapSceneRequest request, ActorResponse response)
		{
			Log.Console("RegisterMapSceneRequestHandler");

			var worldMapComponent = app.GetComponent<WorldMapComponent>();
            worldMapComponent.NormalMaps.Add(worldMapComponent.GetMapConfigId(request.MapType), request.SceneId);

            await ETTask.CompletedTask;
		}
	}
}