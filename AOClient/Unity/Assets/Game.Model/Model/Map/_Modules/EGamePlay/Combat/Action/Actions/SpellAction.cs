﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGamePlay;
using EGamePlay.Combat;
using ET;
#if EGAMEPLAY_ET
using Vector3 = Unity.Mathematics.float3;
#endif

namespace EGamePlay.Combat
{
    public class SpellActionAbility : Entity, IActionAbility
    {
        public CombatEntity OwnerEntity { get { return GetParent<CombatEntity>(); } set { } }
        public bool Enable { get; set; }


        public bool TryMakeAction(out SpellAction action)
        {
            if (Enable == false)
            {
                action = null;
            }
            else
            {
                action = OwnerEntity.AddChild<SpellAction>();
                action.ActionAbility = this;
                action.Creator = OwnerEntity;
            }
            return Enable;
        }
    }

    /// <summary>
    /// 施法行动
    /// </summary>
    public class SpellAction : Entity, IActionExecute
    {
        public SkillAbility SkillAbility { get; set; }
        public SkillExecution SkillExecution { get; set; }
        public List<CombatEntity> SkillTargets { get; set; } = new List<CombatEntity>();
        public CombatEntity InputTarget { get; set; }
        public Vector3 InputPoint { get; set; }
        public float InputDirection { get; set; }

        /// 行动能力
        public Entity ActionAbility { get; set; }
        /// 效果赋给行动源
        public EffectAssignAction SourceAssignAction { get; set; }
        /// 行动实体
        public CombatEntity Creator { get; set; }
        /// 目标对象
        public Entity Target { get; set; }


        public void FinishAction()
        {
            Entity.Destroy(this);
        }

        //前置处理
        private void PreProcess()
        {
            Creator.TriggerActionPoint(ActionPointType.PreSpell, this);
        }

        public void SpellSkill(bool actionOccupy = true)
        {
            PreProcess();
            SkillExecution = SkillAbility.CreateExecution() as SkillExecution;
            SkillExecution.Name = SkillAbility.Name;
            if (SkillTargets.Count > 0)
            {
                SkillExecution.SkillTargets.AddRange(SkillTargets);
            }
            if (SkillAbility.SkillConfig.Id != 2001)
            {
                SkillExecution.ActionOccupy = actionOccupy;
            }
            SkillExecution.InputTarget = InputTarget;
            SkillExecution.InputPoint = InputPoint;
            SkillExecution.InputDirection = InputDirection;
            SkillExecution.BeginExecute();
            AddComponent<UpdateComponent>();
            if (SkillAbility.SkillConfig.Id == 2001)
            {
                SkillExecution.GetParent<CombatEntity>().SpellingExecution = null;
            }
        }

        public override void Update()
        {
            if (SkillExecution != null)
            {
                if (SkillExecution.IsDisposed)
                {
                    PostProcess();
                    FinishAction();
                }
            }
        }

        //后置处理
        private void PostProcess()
        {
            Creator.TriggerActionPoint(ActionPointType.PostSpell, this);
        }
    }
}