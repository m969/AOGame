namespace AO
{
    using AO;
    using ET;
    using EGamePlay.Combat;
    using EGamePlay;
    using TComp = AO.NpcUnit;
    using ET.Server;
    using Unity.Mathematics;
    using SharpCompress.Common;

    public static partial class NpcSystem
    {
        [ObjectSystem]
        public class AwakeHandler : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
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