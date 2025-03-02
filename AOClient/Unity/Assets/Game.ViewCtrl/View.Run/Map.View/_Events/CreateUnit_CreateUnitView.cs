namespace AO
{
    using AssetFile;
    using EGamePlay.Combat;
    using ET;
    using ET.EventType;
    using UnityEngine;

    [Event(SceneType.Process)]
    public class CreateUnit_CreateUnitView : AEvent<CreateUnit>
    {
        protected override async ETTask Run(Entity source, CreateUnit args)
        {
            while (Scene.CurrentScene == null)
            {
                await TimerComponent.Instance.WaitAsync(100);
            }

            var currentScene = Scene.CurrentScene;

            if (args.MapUnit != null)
            {
                args.Unit = args.MapUnit.CreateUnitInfo();
                currentScene.GetComponent<SceneUnitComponent>().Add(args.MapUnit.Entity());
            }
            
            var unitInfo = args.Unit;
            var unitType = (UnitType)unitInfo.UnitType;
            Entity newUnit = null;
            Log.Debug($"CreateUnit_CreateUnitView Run {unitType} {unitInfo.ConfigId} {unitInfo.Name} {unitInfo.Position}");

            if (unitType == UnitType.Actor)
            {
                if (unitInfo.ActorType == ((int)ActorType.Player))
                {
                    if (args.IsMainAvatar)
                    {
                        currentScene = Scene.CurrentScene;
                        newUnit = currentScene.AddChildWithId<Actor>(unitInfo.UnitId);
                        Actor.Main = newUnit as Actor;
                        newUnit.AddComponent<ActorControlComponent>();
                    }
                    else
                    {
                        newUnit = currentScene.AddChildWithId<Actor>(unitInfo.UnitId);
                    }
                }
                else
                {
                    newUnit = currentScene.AddChildWithId<Actor>(unitInfo.UnitId);
                    if (source is ExecutionEditorModeComponent mode)
                    {
                        mode.BossUnit = newUnit as Actor;
                    }
                }

                var actor = newUnit as Actor;
                actor.ActorType = (ActorType)unitInfo.ActorType;
            }

            if (unitType == UnitType.ItemUnit)
            {
                newUnit = currentScene.GetComponent<SceneUnitComponent>().Get(unitInfo.UnitId);
                if (newUnit == null)
                {
                    newUnit = currentScene.AddChildWithId<ItemUnit>(unitInfo.UnitId);
                }
                var itemUnit = newUnit as ItemUnit;
                itemUnit.ItemType = (ItemType)unitInfo.ItemType;
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
                newUnit.MapUnit().SetMapUnitComponents();
                var comps = EntitySystem.DeserializeComponents(unitInfo);
                foreach (var item in comps)
                {
                    Log.Debug(item.GetType().FullName);
                    newUnit.AddComponent(item);
                }
                if (unitInfo.MoveInfo != null && unitInfo.MoveInfo.Points != null && unitInfo.MoveInfo.Points.Count > 0)
                {
                    newUnit.GetComponent<UnitPathMoveComponent>().Speed = unitInfo.MoveInfo.MoveSpeed / 100f;
                    newUnit.MapUnit().MovePathAsync(unitInfo.MoveInfo.Points.ToArray()).Coroutine();
                }
            }

            newUnit.AddComponent<UnitViewComponent>();
            if (newUnit is Actor)
            {
                Log.Debug("newUnit Actor UnitPanelComponent");
                newUnit.AddComponent<UnitPanelComponent>();
            }

            await ETTask.CompletedTask;
        }
    }
}