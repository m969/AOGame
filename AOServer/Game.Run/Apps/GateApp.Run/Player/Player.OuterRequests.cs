namespace AO
{
    using AO;
    using ET;
    using ET.Server;

    public static partial class PlayerOuterRequests
    {
        public static partial async ETTask C2G_EnterMap(Player player, C2G_EnterMap request, G2C_EnterMap response)
        {
            //var player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
            //var player = (Player)playerEntity;
            response.MyId = player.Id;

            var sceneComp = AOGame.MapApp.GetComponent<MapSceneComponent>();
            var map1Scene = sceneComp.GetScene("map1");
            if (map1Scene == null)
            {
                map1Scene = EntitySceneFactory.CreateScene(1, SceneType.Map, "map1", AOGame.RootScene);
                map1Scene.AddComponent<SceneUnitComponent>();
                sceneComp.Add(map1Scene);
            }
            if (player.UnitId == 0)
            {
                CreateNewAvatar(player).Coroutine();
            }
        }

        public static async ETTask CreateNewAvatar(Player player)
        {
            var sceneComp = AOGame.MapApp.GetComponent<MapSceneComponent>();
            var map1Scene = sceneComp.GetScene("map1");
            var unitComp = map1Scene.GetComponent<SceneUnitComponent>();

            var newAvatar = map1Scene.AddChild<Avatar>();
            newAvatar.AddComponent<AvatarGateComponent, long>(player.SessionId);
            newAvatar.AddComponent<MailBoxComponent>();
            newAvatar.AddComponent<UnitMoveComponent>();
            newAvatar.AddComponent<LevelComponent>();
            unitComp.Add(newAvatar);
            player.UnitId = newAvatar.Id;
            var session = Root.Instance.Get(player.SessionId);
            session.GetComponent<SessionPlayerComponent>().AvatarId = newAvatar.Id;

            await TimerComponent.Instance.WaitAsync(100);

            var unitInfo = new UnitInfo() { UnitId = newAvatar.Id, Type = (int)UnitType.Player };
            unitInfo.ComponentInfos = new List<ComponentInfo>();
            var comps = newAvatar.GetNotifyComponents();
            foreach (var comp in comps)
            {
                var compBytes = MongoHelper.Serialize(comp);
                unitInfo.ComponentInfos.Add(new ComponentInfo() { ComponentName = $"{comp.GetType().Name}", ComponentBytes = compBytes });
            }
            MessageHelper.SendToClient(newAvatar, new M2C_CreateMyUnit() { Unit = unitInfo });

            await TimerComponent.Instance.WaitAsync(1000);

            newAvatar.Get<LevelComponent>().Level = 100;
        }
    }
}