using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using EGamePlay.Combat;

namespace EGamePlay.Combat
{
    public class EffectAssignAbility : Entity, IActionAbility
    {
        public CombatEntity OwnerEntity { get { return GetParent<CombatEntity>(); } set { } }
        public bool Enable { get; set; }


        public bool TryMakeAction(out EffectAssignAction action)
        {
            if (Enable == false)
            {
                action = null;
            }
            else
            {
                action = OwnerEntity.AddChild<EffectAssignAction>();
                action.ActionAbility = this;
                action.Creator = OwnerEntity;
            }
            return Enable;
        }
    }

    /// <summary>
    /// ����Ч���ж�
    /// </summary>
    public class EffectAssignAction : Entity, IActionExecute
    {
        /// �������Ч�������ж���Դ����
        public Entity SourceAbility { get; set; }
        /// Ŀ���ж�
        public IActionExecute TargetAction { get; set; }
        public AbilityEffect AbilityEffect { get; set; }
        public Effect EffectConfig => AbilityEffect.EffectConfig;
        /// �ж�����
        public Entity ActionAbility { get; set; }
        /// Ч�������ж�Դ
        public EffectAssignAction SourceAssignAction { get; set; }
        /// �ж�ʵ��
        public CombatEntity Creator { get; set; }
        /// Ŀ�����
        public CombatEntity Target { get; set; }
        /// ����Ŀ��
        public Entity AssignTarget { get; set; }
        /// ����������
        public TriggerContext TriggerContext { get; set; }


        /// ǰ�ô���
        private void PreProcess()
        {
            if (Target == null)
            {
                if (AssignTarget is CombatEntity combatEntity)
                {
                    Target = combatEntity;
                }
                if (AssignTarget is IActionExecute actionExecute)
                {
                    Target = actionExecute.Target;
                }
                if (AssignTarget is SkillExecution skillExecution)
                {
                    Target = skillExecution.InputTarget;
                }
            }
        }

        public void AssignEffect()
        {
            //Log.Debug($"ApplyEffectAssign {EffectConfig}");
            PreProcess();

            foreach (var item in AbilityEffect.Components.Values)
            {
                if (item is IEffectTriggerSystem effectTriggerSystem)
                {
                    effectTriggerSystem.OnTriggerApplyEffect(this);
                }
            }

            PostProcess();

            FinishAction();
        }

        /// ���ô���
        private void PostProcess()
        {
            Creator.TriggerActionPoint(ActionPointType.AssignEffect, this);
            Target.TriggerActionPoint(ActionPointType.ReceiveEffect, this);

            var decorators = AbilityEffect.EffectConfig.Decorators;
            if (decorators != null)
            {
                foreach (var item in decorators)
                {
                    if (item is TriggerNewEffectWhenAssignEffectDecorator effectDecorator)
                    {
                        var newEffect = AbilityEffect.OwnerAbility.GetComponent<AbilityEffectComponent>().GetEffect(((int)effectDecorator.EffectApplyType) - 1);
                        newEffect.TriggerObserver.OnTrigger(Target);
                    }
                }
            }
        }

        public void FinishAction()
        {
            Entity.Destroy(this);
        }
    }
}