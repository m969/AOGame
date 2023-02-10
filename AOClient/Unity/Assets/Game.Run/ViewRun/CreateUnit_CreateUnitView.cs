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
            Log.Debug("CreateUnit_CreateUnitView Run");

            var currentScene = Scene.CurrentScene;
            var unitInfo = args.Unit;
            var unitType = (UnitType)unitInfo.Type;
            Entity newUnit = null;
            Asset asset = null;

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

            var unitComp = currentScene.GetComponent<SceneUnitComponent>();
            unitComp.Add(newUnit);
            newUnit.MapUnit().Position = unitInfo.Position;
            newUnit.AddComponent<UnitTranslateComponent>();
            newUnit.AddComponent<UnitPathMoveComponent>();
            var combatEntity = CombatContext.Instance.AddChild<CombatEntity>();
            combatEntity.Unit = newUnit;
            combatEntity.Position = newUnit.MapUnit().Position;
            newUnit.AddComponent<UnitCombatComponent>().CombatEntity = combatEntity;

            var comps = EntitySystem.DeserializeComponents(unitInfo);
            foreach (var item in comps)
            {
                //Log.Debug(item.GetType().Name);
                newUnit.AddComponent(item);
            }
            newUnit.AddComponent<UnitViewComponent, Asset>(asset);

            await ETTask.CompletedTask;
        }
    }
}