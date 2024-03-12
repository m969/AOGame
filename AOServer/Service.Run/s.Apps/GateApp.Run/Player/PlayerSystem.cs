using AO;
using ET.Server;
using System;
using System.Collections.Generic;

namespace ET
{
	public static class PlayerSystem
    {
        public class AwakeHandler : AwakeSystem<Player, string>
        {
            protected override void Awake(Player self, string account)
            {
               self.Account = account;
            }
        }

        /// <summary>
        /// 从数据库检出角色信息，然后进入场景
        /// </summary>
        public static async ETTask CheckAvatarEnterMap(this Player player)
        {
            Actor myAvatar = null;
            var newAvatar = false;
            if (player.UnitId == 0)
            {
                newAvatar = true;
                var id = IdGenerater.Instance.GenerateUnitId(player.DomainZone());
                myAvatar = ActorFactory.CreateOnGate(ActorType.Player, AOGame.GateApp, id);
                myAvatar.CacheSave();
            }
            else
            {
                myAvatar = await CacheUtils.Query<Actor>(player.UnitId);
                AOGame.GateApp.AddChild(myAvatar);
                var allTypes = EventSystem.Instance.GetTypes();
                var compNames = new List<string>();
                foreach (var item in allTypes.Values)
                {
                    if (item.GetInterface("AO.IUnitDBComponent") == null)
                    {
                        continue;
                    }
                    compNames.Add(item.Name);
                }
                var results = new List<Entity>();
                await CacheUtils.Query(player.UnitId, compNames, results);
                foreach (var item in results)
                {
                    myAvatar.AddComponent(item);
                }
            }

            await player.SendAvatarEnterMap(myAvatar);

            if (newAvatar)
            {
                player.CacheSave();
            }
        }

        /// <summary>
        /// 将角色发送进入地图场景
        /// </summary>
        public static async ETTask SendAvatarEnterMap(this Player player, Actor myAvatar)
        {
            var GateSessionId = player.GetComponent<GateSessionIdComponent>().GateSessionId;

            // 向中心世界服查询场景id
            var getSceneMsg = new GetMapSceneRequest() { MapType = "Map1" };
            var getSceneResponse = await AOZone.GetAppCall<WorldServiceAppCall>().GetMapSceneRequest(getSceneMsg);

            var unitInfo = myAvatar.CreateUnitInfo();
            // 请求进入场景
            var enterSceneMsg = new EnterSceneRequest() { GateSessionId = GateSessionId, UnitInfo = unitInfo, UnitData = MongoHelper.Serialize(myAvatar) };
            var enterSceneResponse = await MessageHelper.CallActor(getSceneResponse.SceneId, enterSceneMsg) as EnterSceneResponse;

            myAvatar.Dispose();

            player.UnitId = enterSceneResponse.UnitId;

            var session = ETRoot.Instance.Get(GateSessionId);
            session.GetComponent<SessionPlayerComponent>().MessageType2EntityId.Add(typeof(IMapMessage), enterSceneResponse.UnitId);
            session.GetComponent<SessionPlayerComponent>().MessageType2ActorId.Add(typeof(IMapMessage), enterSceneResponse.UnitInstanceId);
        }
    }
}
