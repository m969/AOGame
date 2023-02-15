using AO;
using ET.Server;
using System;

namespace ET
{
	public static class SessionPlayerComponentSystem
	{
		public class SessionPlayerComponentDestroySystem: DestroySystem<SessionPlayerComponent>
		{
			protected override async void Destroy(SessionPlayerComponent self)
			{
				// 发送断线消息
				//ActorLocationSenderComponent.Instance?.Send(self.PlayerId, new G2M_SessionDisconnect());
				//MessageHelper.SendToLocationActor(self.AvatarId, new G2M_SessionDisconnect());
				foreach (var item in self.MessageType2ActorId)
				{
					if (item.Value == self.PlayerInstanceId)
					{
						continue;
					}
                    MessageHelper.SendToLocationActor(item.Value, new G2M_SessionDisconnect());
				}
                AOGame.GateApp.GetComponent<PlayerComponent>()?.Remove(self.PlayerId);
			}
		}

		public static Player GetMyPlayer(this SessionPlayerComponent self)
		{
			return AOGame.GateApp.GetComponent<PlayerComponent>().Get(self.PlayerId);
		}
	}
}
