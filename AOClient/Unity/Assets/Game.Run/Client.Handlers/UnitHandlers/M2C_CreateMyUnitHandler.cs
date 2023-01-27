using AO;
using System.Linq.Expressions;

namespace ET.Client
{
	[MessageHandler(SceneType.Client)]
	public class M2C_CreateMyUnitHandler : AMHandler<M2C_CreateMyUnit>
	{
		protected override async ETTask Run(M2C_CreateMyUnit message)
		{
			// 通知场景切换协程继续往下走
			//session.DomainScene().GetComponent<ObjectWait>().Notify(new Wait_CreateMyUnit() { Message = message});
			AOGame.Publish(new AO.EventType.CreateMyUnit() { Unit = message.Unit });
			await ETTask.CompletedTask;
		}
	}
}
