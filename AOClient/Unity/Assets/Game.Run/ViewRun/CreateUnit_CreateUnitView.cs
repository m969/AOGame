namespace AO
{
    using AssetFile;
    using ET;
    using UnityEngine;

    [Event(SceneType.Process)]
    public class CreateUnit_CreateUnitView : AEvent<EventType.CreateUnit>
    {
        protected override async ETTask Run(Scene scene, EventType.CreateUnit args)
        {
            Log.Debug("CreateUnit_CreateUnitView Run");

            var currentScene = Avatar.CurrentScene;
            var unitInfo = args.Unit;
            var unitType = (UnitType)unitInfo.Type;
            Entity newUnit = null;
            Asset asset = null;

            if (unitType == UnitType.Player)
            {
                if (args.IsMainAvatar)
                {
                    await AOGame.ClientApp.GetComponent<MapModeComponent>().ChangeMapScene("map1");
                    currentScene = Avatar.CurrentScene;
                    newUnit = currentScene.AddChildWithId<Avatar>(unitInfo.UnitId);
                    Avatar.Main = newUnit as Avatar;
                    newUnit.AddComponent<AvatarControlComponent>();
                    asset = AssetUtils.LoadAssetAsync("Hero.prefab");
                }
                else
                {
                    newUnit = currentScene.AddChildWithId<Avatar>(unitInfo.UnitId);
                    asset = AssetUtils.LoadAssetAsync("Hero.prefab");
                }
            }
            if (unitType == UnitType.Enemy)
            {
                newUnit = currentScene.AddChildWithId<EnemyUnit>(unitInfo.UnitId);
                asset = AssetUtils.LoadAssetAsync("Enemy.prefab");
            }
            if (unitType == UnitType.Npc)
            {
                newUnit = currentScene.AddChildWithId<EnemyUnit>(unitInfo.UnitId);
                asset = AssetUtils.LoadAssetAsync("Enemy.prefab");
            }

            var unitComp = currentScene.GetComponent<SceneUnitComponent>();
            unitComp.Add(newUnit);
            newUnit.MapUnit().Position = unitInfo.Position;
            newUnit.AddComponent<UnitTranslateComponent>();
            newUnit.AddComponent<UnitPathMoveComponent>();

            var comps = EntitySystem.DeserializeComponents(unitInfo);
            foreach (var item in comps)
            {
                newUnit.AddComponent(item);
            }
            newUnit.AddComponent<UnitViewComponent, Asset>(asset);

            await ETTask.CompletedTask;
        }
    }
}