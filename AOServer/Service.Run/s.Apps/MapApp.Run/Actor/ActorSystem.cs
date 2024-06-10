namespace AO
{
    using AO;
    using ET;
    using EGamePlay.Combat;
    using EGamePlay;
    using TComp = AO.Actor;
    using System.IO;
    using GameUtils;
    using MongoDB.Bson;

    public static class ActorSystem
    {
        [ObjectSystem]
        public class AwakeHandler : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                //self.AddComponent<UnitLevelComponent>();
                //self.AddComponent<AttributeHPComponent>();
                //self.AddComponent<AttributeMPComponent>();

                //self.GetComponent<AttributeHPComponent>().AttributeValue = 100;
                //self.GetComponent<AttributeHPComponent>().AvailableValue = 50;
            }
        }

        public static void ActivateAvatar(this TComp self)
        {
            self.AddComponent<UnitCombatComponent>();

            var combatEntity = CombatContext.Instance.AddChild<CombatEntity>();
            combatEntity.AddComponent<CombatUnitComponent>().Unit = self;
            combatEntity.Position = self.Position;
            self.GetComponent<UnitCombatComponent>().CombatEntity = combatEntity;

            combatEntity.GetComponent<AttributeComponent>().HealthPointMax.SetBase(self.GetComponent<AttributeHPComponent>().AttributeValue);
            combatEntity.GetComponent<AttributeComponent>().HealthPoint.SetBase(self.GetComponent<AttributeHPComponent>().AvailableValue);

            var skillcfg = AssetUtils.LoadObject<SkillConfigObject>("SkillConfigs/Skill_1002");
            var skill = self.GetComponent<UnitCombatComponent>().CombatEntity.AttachSkill(skillcfg);
            skillcfg = AssetUtils.LoadObject<SkillConfigObject>("SkillConfigs/Skill_1003");
            skill = self.GetComponent<UnitCombatComponent>().CombatEntity.AttachSkill(skillcfg);

            self.EnterState<IdleState>();
        }
    }
}