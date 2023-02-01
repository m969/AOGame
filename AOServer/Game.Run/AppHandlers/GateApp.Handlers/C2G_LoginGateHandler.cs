using AO;
using System;


namespace ET.Server
{
	[MessageHandler(SceneType.Gate)]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		protected override async ETTask Run(Entity session, C2G_LoginGate request, G2C_LoginGate response)
		{
			//Scene scene = session.DomainScene();
			//string account = scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
			//if (account == null)
			//{
			//	response.Error = ErrorCore.ERR_ConnectGateKeyError;
			//	response.Message = "Gate key验证失败!";
			//	return;
			//}

			//session.RemoveComponent<SessionAcceptTimeoutComponent>();

			var account = request.Key.ToString();
            var playerComp = AOGame.GateApp.GetComponent<PlayerComponent>();
			var player = playerComp.AddChild<Player, string>(account);
			playerComp.Add(player);
			session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;
			session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
			player.SessionId = session.InstanceId;

            player.AddComponent<MailBoxComponent>();

            response.PlayerId = player.Id;
			await ETTask.CompletedTask;
		}
	}
}