using AO;
using ET.Server;
using System;

namespace ET
{
	public static class PlayerSystem
    {
        public static async ETTask CheckAvatarEnterMap(this Player player)
        {
            var GateSessionId = player.GetComponent<GateSessionIdComponent>().GateSessionId;
            var myAvatar = AOGame.Root.AddChildWithId<Avatar>(IdGenerater.Instance.GenerateUnitId(player.DomainZone()));

            // 向中心世界服查询场景id
            var getSceneMsg = new GetMapSceneRequest() { MapType = "Map1" };
            var getSceneResponse = await AOZone.GetAppCall<WorldServiceAppCall>().GetMapSceneRequest(getSceneMsg);

            // 请求进入场景
            var enterSceneMsg = new EnterSceneRequest() { GateSessionId = GateSessionId, UnitData = MongoHelper.Serialize(myAvatar) };
            var enterSceneResponse = await AOZone.GetEntityCall<SceneCall>(getSceneResponse.SceneId).EnterSceneRequest(enterSceneMsg);

            myAvatar.Dispose();

            player.UnitId = enterSceneResponse.UnitId;
            var session = ETRoot.Instance.Get(GateSessionId);
            session.GetComponent<SessionPlayerComponent>().MessageType2EntityId.Add(typeof(IMapMessage), enterSceneResponse.UnitId);
            session.GetComponent<SessionPlayerComponent>().MessageType2ActorId.Add(typeof(IMapMessage), enterSceneResponse.UnitInstanceId);
        }
    }
}
