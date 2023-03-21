namespace AO
{
    using AO;
    using ET;
    using EGamePlay.Combat;
    using EGamePlay;
    using TComp = AO.Avatar;
    using System.IO;
    using GameUtils;
    using MongoDB.Bson;

    public static class AvatarSystem
    {
        [ObjectSystem]
        public class AvatarAwakeSystem : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                self.SetMapUnitComponents();

                self.AddComponent<UnitLevelComponent>();
                self.AddComponent<AttributeHPComponent>();
                self.AddComponent<UnitCombatComponent>();

                self.GetComponent<AttributeHPComponent>().Attribute_HP = 100;
                self.GetComponent<AttributeHPComponent>().Available_HP = 50;

                var combatEntity = CombatContext.Instance.AddChild<CombatEntity>();
                combatEntity.Unit = self;
                combatEntity.Position = self.Position;
                self.GetComponent<UnitCombatComponent>().CombatEntity = combatEntity;

                combatEntity.GetComponent<AttributeComponent>().HealthPointMax.SetBase(self.GetComponent<AttributeHPComponent>().Attribute_HP);
                combatEntity.GetComponent<AttributeComponent>().HealthPoint.SetBase(self.GetComponent<AttributeHPComponent>().Available_HP);

                var skillcfg = AssetUtils.Load<SkillConfigObject>("Skill_1002");
                var skill = self.GetComponent<UnitCombatComponent>().CombatEntity.AttachSkill(skillcfg);
                skillcfg = AssetUtils.Load<SkillConfigObject>("Skill_1003");
                skill = self.GetComponent<UnitCombatComponent>().CombatEntity.AttachSkill(skillcfg);

                self.EnterState<IdleState>();
            }
        }
    }
}