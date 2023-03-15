using AO;
using cfg.Map;
using EGamePlay.Combat;
using System;


namespace ET.Server
{
    [ActorMessageHandler(SceneType.Gate)]
	public class GetMapSceneRequestHandler : AMActorRpcHandler<WorldServiceApp, GetMapSceneRequest, GetMapSceneResponse>
	{
		protected override async ETTask Run(WorldServiceApp app, GetMapSceneRequest request, GetMapSceneResponse response)
		{
			Log.Console("GetMapSceneRequestHandler");

            var worldMapComponent = app.GetComponent<WorldMapComponent>();
            if (worldMapComponent.NormalMaps.TryGetValue(worldMapComponent.GetMapConfigId(request.MapType), out var sceneId))
			{
				response.SceneId = sceneId;
			}

            await ETTask.CompletedTask;
		}
	}
}