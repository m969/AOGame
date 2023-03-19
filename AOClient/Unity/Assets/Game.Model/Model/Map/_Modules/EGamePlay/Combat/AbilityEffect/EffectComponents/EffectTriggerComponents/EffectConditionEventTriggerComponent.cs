using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 条件事件触发组件
    /// </summary>
    public class EffectConditionEventTriggerComponent : Component
    {
        public override bool DefaultEnable { get; set; } = false;
        public string ConditionParamValue { get; set; }


        public override void OnEnable()
        {
            var conditionType = GetEntity<EffectTriggerEventBind>().GetParent<AbilityEffect>().EffectConfig.ConditionType;
            var conditionParam = ConditionParamValue;
            GetEntity<EffectTriggerEventBind>().OwnerEntity.ListenerCondition(conditionType, OnConditionTrigger, conditionParam);
        }

        private void OnConditionTrigger()
        {
            //GetEntity<AbilityEffect>().TryAssignEffectToOwner();
            //GetEntity<EffectTriggerEventBind>().TriggerSelfEffectCheck();
        }
    }
}