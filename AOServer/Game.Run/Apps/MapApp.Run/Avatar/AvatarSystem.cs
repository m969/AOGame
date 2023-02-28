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

                //var skillcfg = new SkillConfigObject();
                //skillcfg.Id = 1002;
                //skillcfg.Name = "1002";
                //var damage = new DamageEffect();
                //damage.DamageValueFormula = "100";
                //skillcfg.Effects.Add(damage);

                var skillcfg = AssetUtils.Load<SkillConfigObject>("Skill_1002");
                //ET.Log.Console(skillcfg.ToJson());
                //foreach (var item in skillcfg.Effects)
                //{
                //    ET.Log.Console($"{item.GetType()}");
                //}
                var skill = self.GetComponent<UnitCombatComponent>().CombatEntity.AttachSkill(skillcfg);

                //var text = File.ReadAllText("../../SkillConfigs/Execution_1002.json");
                //var skillexc = JsonHelper.FromJson<ExecutionObject>(text);
                //skill.ExecutionObject = skillexc;
                //ET.Log.Console($"AvatarAwakeSystem skillexc={skillexc.Id}");
            }
        }
    }
}