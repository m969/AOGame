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
        public static void Create(ActorType actorType, Scene scene)
        {
            var self = scene.AddChild<Actor>();
            scene.GetComponent<SceneUnitComponent>().Add(self);

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
        }
    }
}