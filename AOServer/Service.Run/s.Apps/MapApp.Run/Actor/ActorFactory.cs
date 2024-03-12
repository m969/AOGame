namespace AO
{
    using AO;
    using ET;
    using EGamePlay.Combat;
    using EGamePlay;
    using System.IO;
    using GameUtils;
    using MongoDB.Bson;
    using Unity.Mathematics;

    public static class ActorFactory
    {
        public static Actor CreateOnGate(ActorType actorType, GateApp app, long id = 0)
        {
            Actor self;
            if (id == 0)
            {
                self = app.AddChild<Actor>();
            }
            else
            {
                self = app.AddChildWithId<Actor>(id);
            }

            self.ActorType = actorType;
            //self.Position = new float3(100, 100, 0);

            if (actorType == ActorType.Player)
            {
                self.AddComponent<UnitLevelComponent>();
                self.AddComponent<AttributeHPComponent>();
                self.AddComponent<AttributeMPComponent>();

                self.GetComponent<AttributeHPComponent>().AttributeValue = 100;
                self.GetComponent<AttributeHPComponent>().AvailableValue = 50;
            }
            return self;
        }

        public static Actor Create(ActorType actorType, Scene scene, long id = 0)
        {
            Actor self;
            if (id == 0)
            {
                self = scene.AddChild<Actor>();
            }
            else
            {
                self = scene.AddChildWithId<Actor>(id);
            }

            self.ActorType = actorType;
            //self.Position = new float3(100, 100, 0);

            if (actorType == ActorType.Player)
            {
                self.AddComponent<UnitLevelComponent>();
                self.AddComponent<AttributeHPComponent>();
                self.AddComponent<AttributeMPComponent>();

                self.GetComponent<AttributeHPComponent>().AttributeValue = 100;
                self.GetComponent<AttributeHPComponent>().AvailableValue = 50;
            }
            if (actorType == ActorType.NonPlayer)
            {
                self.SetMapUnitComponents();

                self.AddComponent<UnitSpawnPointComponent, float3>(self.Position);
                self.AddComponent<UnitStateMachine>();
                self.AddComponent<AIStateMachine>();
                self.AddComponent<UnitLevelComponent>();
                self.AddComponent<AttributeHPComponent>();
                self.AddComponent<UnitCombatComponent>();

                self.GetComponent<UnitPathMoveComponent>().Speed = 1.3f;
                self.GetComponent<AttributeHPComponent>().AttributeValue = 100;
                self.GetComponent<AttributeHPComponent>().AvailableValue = 100;

                var combatEntity = CombatContext.Instance.AddChild<CombatEntity>();
                combatEntity.Unit = self;
                combatEntity.Position = self.Position;
                self.GetComponent<UnitCombatComponent>().CombatEntity = combatEntity;

                combatEntity.GetComponent<AttributeComponent>().HealthPointMax.SetBase(self.GetComponent<AttributeHPComponent>().AttributeValue);
                combatEntity.GetComponent<AttributeComponent>().HealthPoint.SetBase(self.GetComponent<AttributeHPComponent>().AvailableValue);

                self.EnterState<IdleState>();
                self.EnterAI<PatrolAI>();
            }

            return self;
        }
    }
}