namespace AO
{
    using AssetFile;
    using EGamePlay.Combat;
    using ET;
    using UnityEngine;

    [Event(SceneType.Process)]
    public class CreateUnit_CreateUnitView : AEvent<EventType.CreateUnit>
    {
        protected override async ETTask Run(Entity source, EventType.CreateUnit args)
        {
            var currentScene = Scene.CurrentScene;

            if (args.MapUnit != null)
            {
                args.Unit = args.MapUnit.CreateUnitInfo();
                currentScene.GetComponent<SceneUnitComponent>().Add(args.MapUnit.Entity());
            }

            var unitInfo = args.Unit;
            var unitType = (UnitType)unitInfo.Type;
            Entity newUnit = null;
            Asset asset = null;
            Log.Debug($"CreateUnit_CreateUnitView Run {unitInfo.Type} {unitInfo.ConfigId} {unitInfo.Name}");

            if (unitType == UnitType.Player)
            {
                if (args.IsMainAvatar)
                {
                    if (currentScene == null || currentScene.Type != "ExecutionLinkScene")
                    {
                        await AOGame.ClientApp.GetComponent<MapModeComponent>().ChangeMapScene("Map1");
                    }
                    currentScene = Scene.CurrentScene;
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
                if (source is ExecutionEditorModeComponent mode)
                {
                    mode.BossUnit = newUnit as EnemyUnit;
                }
            }
            if (unitType == UnitType.Npc)
            {
                newUnit = currentScene.AddChildWithId<EnemyUnit>(unitInfo.UnitId);
                asset = AssetUtils.LoadAssetAsync("Enemy.prefab");
            }
            if (unitType == UnitType.ItemUnit)
            {
                newUnit = currentScene.GetComponent<SceneUnitComponent>().Get(unitInfo.UnitId);
                if (newUnit == null)
                {
                    newUnit = currentScene.AddChildWithId<ItemUnit>(unitInfo.UnitId);
                }
                asset = AssetUtils.LoadAssetAsync("ItemUnit.prefab");
            }

            var unitComp = currentScene.GetComponent<SceneUnitComponent>();
            if (unitComp.Get(newUnit.Id) == null)
            {
                unitComp.Add(newUnit);
            }
            newUnit.MapUnit().Position = unitInfo.Position;
            newUnit.MapUnit().ConfigId = unitInfo.ConfigId;
            newUnit.MapUnit().Name = unitInfo.Name;
            if (args.MapUnit == null)
            {
                newUnit.AddComponent<UnitTranslateComponent>();
                var comps = EntitySystem.DeserializeComponents(unitInfo);
                foreach (var item in comps)
                {
                    //Log.Debug(item.GetType().Name);
                    newUnit.AddComponent(item);
                }
                if (unitInfo.MoveInfo != null && unitInfo.MoveInfo.Points.Count > 0)
                {
                    newUnit.MapUnit().MovePathAsync(unitInfo.MoveInfo.Points.ToArray()).Coroutine();
                }
            }

            newUnit.AddComponent<UnitViewComponent, Asset>(asset);

            await ETTask.CompletedTask;
        }
    }
}