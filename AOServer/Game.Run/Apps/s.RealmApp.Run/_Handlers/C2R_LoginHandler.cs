using AO;
using System;
using System.Numerics;


namespace ET.Server
{
	[MessageHandler(SceneType.Realm)]
	public class C2R_LoginHandler : AMRpcHandler<C2R_Login, R2C_Login>
	{
		protected override async ETTask Run(Entity session, C2R_Login request, R2C_Login response)
		{
			response.Key = RandomGenerator.RandInt64();
			response.GateId = AOZone.GetAppId<GateApp>();
            await ETTask.CompletedTask;
		}
	}
}