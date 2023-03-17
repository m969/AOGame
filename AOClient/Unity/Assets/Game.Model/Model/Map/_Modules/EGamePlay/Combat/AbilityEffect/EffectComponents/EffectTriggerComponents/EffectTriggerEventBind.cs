using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 效果触发事件绑定
    /// </summary>
    public class EffectTriggerEventBind : Entity
    {
        public CombatEntity OwnerEntity => GetParent<AbilityEffect>().OwnerEntity;
        public IAbilityEntity OwnerAbility => GetParent<AbilityEffect>().OwnerAbility as IAbilityEntity;
        public SkillAbility SkillAbility => (OwnerAbility as SkillAbility);
        public string AffectCheck { get; set; }
        public List<IConditionCheckSystem> ConditionChecks { get; set; } = new List<IConditionCheckSystem>();
        public List<Entity> ActionPointBinds { get; set; } = new List<Entity>();


        public override void Awake()
        {
            //Log.Debug($"EffectTriggerEventBind Awake {affectCheck}");
            var effectConfig = GetParent<AbilityEffect>().EffectConfig;
            /// 行动点事件
            var isAction = effectConfig.EffectTriggerType == EffectTriggerType.Action;
            if (isAction) AddComponent<EffectActionPointTriggerComponent>();
            /// 条件事件
            var isCondition = effectConfig.EffectTriggerType == EffectTriggerType.Condition && !string.IsNullOrEmpty(effectConfig.ConditionParam);
            if (isCondition) AddComponent<EffectConditionEventTriggerComponent>();

            if (effectConfig.ConditionParam != null)
            {
                //AddChild<EffectConditionEventTriggerComponent>();
            }
        }

        public override void OnDestroy()
        {
            foreach (var item in ConditionChecks)
            {
                Entity.Destroy(item as Entity);
            }
            ConditionChecks.Clear();
            foreach (var item in ActionPointBinds)
            {
                Entity.Destroy(item);
            }
            ActionPointBinds.Clear();
            base.OnDestroy();
        }

        public void TriggerEffectCheck(SkillExecution skillExecution)
        {
            var affectCheck = GetParent<AbilityEffect>().EffectConfig.ConditionParam;
            //Log.Debug($"EffectTriggerEventBind TriggerEffectCheck {affectCheck}");
            //if (GetParent<AbilityEffect>().GetComponent<EffectTargetSelection>() == null && GetParent<AbilityEffect>().IsTargetEffect)
            //{
            //    foreach (var target in skillExecution.MainTargetBattlers)
            //    {
            //        TriggerEffectCheckWithTarget(target);
            //    }
            //}
            //else
            {
                TriggerEffectCheckWithTarget(skillExecution);
            }
        }

        public void TriggerEffectCheckWithTarget(Entity target)
        {
            var affectCheck = GetParent<AbilityEffect>().EffectConfig.ConditionParam;
            //Log.Debug($"EffectTriggerEventBind TriggerEffectCheckWithTarget {affectCheck}");
            var conditionCheckResult = true;
            foreach (var item in ConditionChecks)
            {
                if (item.IsInvert)
                {
                    if (item.CheckCondition(target))
                    {
                        conditionCheckResult = false;
                        break;
                    }
                }
                else
                {
                    if (!item.CheckCondition(target))
                    {
                        conditionCheckResult = false;
                        break;
                    }
                }
            }

            if (conditionCheckResult)
            {
                //var effectTargetSelection = GetParent<AbilityEffect>().GetComponent<EffectTargetSelection>();
                //if (effectTargetSelection != null)
                //{
                //    var targets = effectTargetSelection.GetTargets();
                //    foreach (var battler in targets)
                //    {
                //        GetParent<AbilityEffect>().AssignEffectTo(battler);
                //    }
                //}
                //else
                {
                    GetParent<AbilityEffect>().TriggerEffect(target);
                }
            }
        }

        public void TriggerSelfEffectCheck()
        {
            TriggerEffectCheckWithTarget(OwnerEntity);
        }
    }
}