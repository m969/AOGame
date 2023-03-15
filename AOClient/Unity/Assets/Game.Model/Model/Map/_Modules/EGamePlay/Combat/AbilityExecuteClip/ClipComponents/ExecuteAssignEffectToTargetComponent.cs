using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameUtils;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 执行体应用效果给目标组件
    /// </summary>
    public class ExecuteAssignEffectToTargetComponent : Component
    {
        public override bool DefaultEnable { get; set; } = false;
        public EffectApplyType EffectApplyType { get; set; }


        public override void Awake()
        {
            Entity.Subscribe<ExecuteEffectEvent>(OnTriggerExecuteEffect);
        }

        public void OnTriggerExecuteEffect(ExecuteEffectEvent evnt)
        {
#if UNITY
            var skillExecution = Entity.GetParent<SkillExecution>();
            Log.Debug($"ExecutionAssignToTargetComponent OnTriggerExecutionEffect {skillExecution.InputTarget} {EffectApplyType}");
            if (skillExecution.InputTarget != null)
            {
                var abilityEffectComponent = skillExecution.AbilityEntity.GetComponent<AbilityEffectComponent>();
                var OwnerEntity = skillExecution.OwnerEntity;
                if (EffectApplyType == EffectApplyType.AllEffects)
                {
                    foreach (var abilityEffect in abilityEffectComponent.AbilityEffects)
                    {
                        if (OwnerEntity.EffectAssignAbility.TryMakeAction(out var action))
                        {
                            //Log.Debug($"AbilityEffect TryAssignEffectTo {targetEntity} {EffectConfig}");
                            action.AssignTarget = skillExecution.InputTarget;
                            action.SourceAbility = skillExecution.AbilityEntity;
                            action.AbilityEffect = abilityEffect;
                            action.AssignEffect();
                        }
                    }
                }
                else
                {
                    var abilityEffect = abilityEffectComponent.GetEffect((int)EffectApplyType - 1);
                    if (OwnerEntity.EffectAssignAbility.TryMakeAction(out var action))
                    {
                        //Log.Debug($"AbilityEffect TryAssignEffectTo {targetEntity} {EffectConfig}");
                        action.AssignTarget = skillExecution.InputTarget;
                        action.SourceAbility = skillExecution.AbilityEntity;
                        action.AbilityEffect = abilityEffect;
                        action.AssignEffect();
                    }
                }
            }
#endif
        }
    }
}