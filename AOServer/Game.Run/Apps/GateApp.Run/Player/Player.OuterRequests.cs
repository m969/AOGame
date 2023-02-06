namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static partial class PlayerOuterRequests
    {
        public static partial async ETTask C2G_EnterMap(Player player, C2G_EnterMap request, G2C_EnterMap response)
        {
            response.MyId = player.Id;
            if (player.UnitId == 0)
            {
                CreateNewAvatar(player).Coroutine();
            }
            await ETTask.CompletedTask;
        }

        public static async ETTask CreateNewAvatar(Player player)
        {
            var sceneComp = AOGame.MapApp.GetComponent<MapSceneComponent>();
            var map1Scene = sceneComp.GetScene("map1");
            var unitComp = map1Scene.GetComponent<SceneUnitComponent>();

            var newAvatar = map1Scene.AddChild<Avatar>();

            unitComp.Add(newAvatar);
            player.UnitId = newAvatar.Id;
            var session = Root.Instance.Get(player.GetComponent<GateSessionIdComponent>().GateSessionId);
			session.GetComponent<SessionPlayerComponent>().MessageType2Actor.Add(typeof(IMapMessage), newAvatar.Id);
            newAvatar.AddComponent<AvatarCall, long>(session.InstanceId);

            await TimerComponent.Instance.WaitAsync(100);

            var unitInfo = newAvatar.CreateUnitInfo();
            unitInfo.ComponentInfos = new List<ComponentInfo>();
            var comps = newAvatar.GetNotifyComponents();
            foreach (var comp in comps)
            {
                var compBytes = MongoHelper.Serialize(comp);
                unitInfo.ComponentInfos.Add(new ComponentInfo() { ComponentName = $"{comp.GetType().Name}", ComponentBytes = compBytes });
            }
            newAvatar.ClientCall.M2C_CreateMyUnit(new M2C_CreateMyUnit() { Unit = unitInfo });

            await TimerComponent.Instance.WaitAsync(1000);

            var msg = new M2C_CreateUnits() { Units = new List<UnitInfo>() };
            foreach (IMapUnit item in unitComp.GetAll())
            {
                if (item == newAvatar) continue;
                msg.Units.Add(item.CreateUnitInfo());
            }
            newAvatar.ClientCall.M2C_CreateUnits(msg);

            newAvatar.GetComponent<UnitLevelComponent>().Level = 100;
        }
    }
}