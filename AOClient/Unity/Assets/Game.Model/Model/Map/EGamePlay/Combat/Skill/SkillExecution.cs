using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using EGamePlay.Combat;
using ET;
using Log = EGamePlay.Log;
using Unity.Mathematics;
using System;
using AO;
using AO.EventType;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 技能执行体，执行体就是控制角色表现和技能表现的，包括角色动作、移动、变身等表现的，以及技能生成碰撞体等表现
    /// </summary>
    [EnableUpdate]
    public sealed partial class SkillExecution : Entity, IAbilityExecution
    {
        public Entity AbilityEntity { get; set; }
        public CombatEntity OwnerEntity { get; set; }
        public SkillAbility SkillAbility { get { return AbilityEntity as SkillAbility; } }
        public ExecutionObject ExecutionObject { get; set; }
        public List<CombatEntity> SkillTargets { get; set; } = new List<CombatEntity>();
        public CombatEntity InputTarget { get; set; }
        public float3 InputPoint { get; set; }
        public float InputDirection { get; set; }
        public long OriginTime { get; set; }
        /// 行为占用
        public bool ActionOccupy { get; set; } = true;


        public override void Awake(object initData)
        {
            AbilityEntity = initData as SkillAbility;
            OwnerEntity = GetParent<CombatEntity>();
            OriginTime = ET.TimeHelper.ServerNow();
        }

        public void LoadExecutionEffects()
        {
            AddComponent<ExecutionClipComponent>();
        }

        public override void Update()
        {
            //if (SkillAbility.Spelling == false)
            //{
            //    return;
            //}

            var nowSeconds = (double)(ET.TimeHelper.ClientNow() - OriginTime) / 1000;

            if (nowSeconds >= ExecutionObject.TotalTime)
            {
                EndExecute();
            }
        }

        public void BeginExecute()
        {
            GetParent<CombatEntity>().SpellingExecution = this;
            if (SkillAbility != null)
            {
                SkillAbility.Spelling = true;
            }

            GetComponent<ExecutionClipComponent>().BeginExecute();

            FireEvent(nameof(BeginExecute));
        }

        public void EndExecute()
        {
            //Log.Debug("SkillExecution EndExecute");
            GetParent<CombatEntity>().SpellingExecution = null;
            if (SkillAbility != null)
            {
                SkillAbility.Spelling = false;
            }
            SkillTargets.Clear();
            Entity.Destroy(this);
            //base.EndExecute();
        }
    }
}