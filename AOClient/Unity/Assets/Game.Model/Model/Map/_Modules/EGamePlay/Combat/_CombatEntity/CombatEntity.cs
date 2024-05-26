﻿using EGamePlay.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace EGamePlay.Combat
{
    public class EntityDeadEvent { public CombatEntity DeadEntity; }

    /// <summary>
    /// 战斗实体
    /// </summary>
    public sealed class CombatEntity : Entity, IPosition
    {
        public ET.Entity Unit { get; set; }
        public GameObject HeroObject { get; set; }
        public Transform ModelTrans { get; set; }
        public HealthPoint CurrentHealth { get; private set; }

        //效果赋给行动能力
        public EffectAssignAbility EffectAssignAbility { get; private set; }
        //施法行动能力
        public SpellActionAbility SpellAbility { get; private set; }
        //移动行动能力
        public MotionActionAbility MotionAbility { get; private set; }
        //伤害行动能力
        public DamageActionAbility DamageAbility { get; private set; }
        //治疗行动能力
        public CureActionAbility CureAbility { get; private set; }
        //施加状态行动能力
        public AddStatusActionAbility AddStatusAbility { get; private set; }
        //施法普攻行动能力
        public AttackActionAbility AttackSpellAbility { get; private set; }
        //回合行动能力
        public RoundActionAbility RoundAbility { get; private set; }
        //起跳行动能力
        public JumpToActionAbility JumpToAbility { get; private set; }

        //普攻能力
        public AttackAbility AttackAbility { get; set; }
        //普攻格挡能力
        public AttackBlockActionAbility AttackBlockAbility { get; set; }

        //执行中的执行体
        public SkillExecution SpellingExecution { get; set; }
        public Dictionary<string, SkillAbility> NameSkills { get; set; } = new Dictionary<string, SkillAbility>();
        public Dictionary<int, SkillAbility> IdSkills { get; set; } = new Dictionary<int, SkillAbility>();
        public Dictionary<KeyCode, SkillAbility> InputSkills { get; set; } = new Dictionary<KeyCode, SkillAbility>();
        public Dictionary<string, List<StatusAbility>> TypeIdStatuses { get; set; } = new Dictionary<string, List<StatusAbility>>();
        public float3 Position { get; set; }
        public float3 Rotation { get; set; }
        /// 行为禁制
        public ActionControlType ActionControlType { get; set; }
        /// 禁制豁免
        public ActionControlType ActionControlImmuneType { get; set; }


        public override void Awake()
        {
            AddComponent<AttributeComponent>();
            AddComponent<ActionPointComponent>();
            //AddComponent<ConditionEventComponent>();
            AddComponent<StatusComponent>();
            AddComponent<SkillComponent>();
            AddComponent<SpellComponent>();
            AddComponent<MotionComponent>();
            CurrentHealth = AddChild<HealthPoint>();
            CurrentHealth.HealthPointNumeric = GetComponent<AttributeComponent>().HealthPoint;
            CurrentHealth.HealthPointMaxNumeric = GetComponent<AttributeComponent>().HealthPointMax;
            CurrentHealth.Reset();

            AttackAbility = AttachAbility<AttackAbility>(null);
            AttackBlockAbility = AttachAction<AttackBlockActionAbility>();

            EffectAssignAbility = AttachAction<EffectAssignAbility>();
            SpellAbility = AttachAction<SpellActionAbility>();
            MotionAbility = AttachAction<MotionActionAbility>();
            DamageAbility = AttachAction<DamageActionAbility>();
            CureAbility = AttachAction<CureActionAbility>();
            AddStatusAbility = AttachAction<AddStatusActionAbility>();
            AttackSpellAbility = AttachAction<AttackActionAbility>();
            RoundAbility = AttachAction<RoundActionAbility>();
            JumpToAbility = AttachAction<JumpToActionAbility>();
        }

        #region 行动点事件
        public void ListenActionPoint(ActionPointType actionPointType, Action<Entity> action)
        {
            GetComponent<ActionPointComponent>().AddListener(actionPointType, action);
        }

        public void UnListenActionPoint(ActionPointType actionPointType, Action<Entity> action)
        {
            GetComponent<ActionPointComponent>().RemoveListener(actionPointType, action);
        }

        public void TriggerActionPoint(ActionPointType actionPointType, Entity action)
        {
            GetComponent<ActionPointComponent>().TriggerActionPoint(actionPointType, action);
        }
        #endregion

        //#region 条件事件
        //public void ListenCondition(TimeStateEventType conditionType, Action action, object paramObj = null)
        //{
        //    GetComponent<ConditionEventComponent>().AddListener(conditionType, action, paramObj);
        //}

        //public void UnListenCondition(TimeStateEventType conditionType, Action action)
        //{
        //    GetComponent<ConditionEventComponent>().RemoveListener(conditionType, action);
        //}
        //#endregion

        public void ReceiveDamage(IActionExecute combatAction)
        {
            var damageAction = combatAction as DamageAction;
            CurrentHealth.Minus(damageAction.DamageValue);
        }

        public void ReceiveCure(IActionExecute combatAction)
        {
            var cureAction = combatAction as CureAction;
            CurrentHealth.Add(cureAction.CureValue);
        }

        public bool CheckDead()
        {
            return CurrentHealth.Value <= 0;
        }

        /// <summary>
        /// 挂载能力，技能、被动、buff等都通过这个接口挂载
        /// </summary>
        /// <param name="configObject"></param>
        public T AttachAbility<T>(object configObject) where T : Entity, IAbilityEntity
        {
            var ability = this.AddChild<T>(configObject);
            ability.AddComponent<AbilityLevelComponent>();
            return ability;
        }

        public T AttachAction<T>() where T : Entity, IActionAbility
        {
            var action = AddChild<T>();
            action.AddComponent<ActionComponent>();
            action.Enable = true;
            //var action = AttachAbility<T>(null);
            //action.TryActivateAbility();
            return action;
        }

        public SkillAbility AttachSkill(object configObject)
        {
            var skill = AttachAbility<SkillAbility>(configObject);
            NameSkills.Add(skill.SkillConfig.Name, skill);
            IdSkills.Add(skill.SkillConfig.Id, skill);
            return skill;
        }

        public StatusAbility AttachStatus(object configObject)
        {
            return GetComponent<StatusComponent>().AttachStatus(configObject);
        }

        public void OnStatusRemove(StatusAbility statusAbility)
        {
            GetComponent<StatusComponent>().OnStatusRemove(statusAbility);
        }

        public void BindSkillInput(SkillAbility abilityEntity, KeyCode keyCode)
        {
            InputSkills.Add(keyCode, abilityEntity);
            abilityEntity.TryActivateAbility();
        }

        public bool HasStatus(string statusTypeId)
        {
            return GetComponent<StatusComponent>().TypeIdStatuses.ContainsKey(statusTypeId);
        }

        public StatusAbility GetStatus(string statusTypeId)
        {
            return GetComponent<StatusComponent>().TypeIdStatuses[statusTypeId][0];
        }

        #region 回合制战斗
        public int SeatNumber { get; set; }
        public int JumpToTime { get; set; }
        public bool IsHero { get; set; }
        public bool IsMonster => IsHero == false;

        public CombatEntity GetEnemy(int seat)
        {
            if (IsHero)
            {
                return GetParent<CombatContext>().GetMonster(seat);
            }
            else
            {
                return GetParent<CombatContext>().GetHero(seat);
            }
        }

        public CombatEntity GetTeammate(int seat)
        {
            if (IsHero)
            {
                return GetParent<CombatContext>().GetHero(seat);
            }
            else
            {
                return GetParent<CombatContext>().GetMonster(seat);
            }
        }
        #endregion
    }

    public class RemoveStatusEvent
    {
        public CombatEntity CombatEntity { get; set; }
        public StatusAbility Status { get; set; }
        public long StatusId { get; set; }
    }
}