namespace AO
{
    using AO;
    using ET;
    using EGamePlay.Combat;
    using EGamePlay;
    using TComp = AO.EnemyUnit;

    public static partial class EnemyUnitSystem
    {
        [ObjectSystem]
        public class EnemyUnitAwakeSystem : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                self.AddComponent<UnitTranslateComponent>();
                self.AddComponent<UnitPathMoveComponent>();
                self.AddComponent<UnitLevelComponent>();
                self.AddComponent<AttributeHPComponent>();
                self.AddComponent<UnitCombatComponent>();

                self.GetComponent<AttributeHPComponent>().Attribute_HP = 100;
                self.GetComponent<AttributeHPComponent>().Available_HP = 100;

                var combatEntity = CombatContext.Instance.AddChild<CombatEntity>();
                combatEntity.Unit = self;
                combatEntity.Position = self.Position;
                self.GetComponent<UnitCombatComponent>().CombatEntity = combatEntity;

                combatEntity.GetComponent<AttributeComponent>().HealthPointMax.SetBase(self.GetComponent<AttributeHPComponent>().Attribute_HP);
                combatEntity.GetComponent<AttributeComponent>().HealthPoint.SetBase(self.GetComponent<AttributeHPComponent>().Available_HP);
            }
        }
    }
}