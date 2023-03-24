using AO;
using System;
using System.Numerics;


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

			var account = request.Account;
            var playerComp = AOGame.GateApp.GetComponent<PlayerComponent>();

			Player player = null;
			var results = await DBCacheUtils.Query<Player>((AOGame.DomainApp as IApp).Zone, (p) => p.Account == account);
			if (results.Count > 0)
			{
                player = results[0];
                if (AOGame.GateApp.GetChild<Player>(player.Id) != null)
                {
                    AOGame.GateApp.RemoveChild(player.Id);
                }
                AOGame.GateApp.AddChild(player);
            }
			else
			{
				player = playerComp.AddChildWithId<Player, string>(IdGenerater.Instance.GenerateUnitId(session.DomainZone()), account);
                player.CacheSave();
            }

			if (playerComp.Get(player.Id) != null)
			{
				playerComp.Remove(player.Id);
			}
            playerComp.Add(player);
			player.AddComponent<PlayerClient, long>(session.InstanceId);
            session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;
            session.GetComponent<SessionPlayerComponent>().PlayerInstanceId = player.InstanceId;
            session.GetComponent<SessionPlayerComponent>().MessageType2EntityId.Add(typeof(IGateMessage), player.Id);
            session.GetComponent<SessionPlayerComponent>().MessageType2ActorId.Add(typeof(IGateMessage), player.InstanceId);
            session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);

			EventSystem.Instance.Publish(session, new AO.EventType.SessionRemoveAcceptTimeoutComponentEvent());

            response.PlayerId = player.Id;
			await ETTask.CompletedTask;
		}
	}
}