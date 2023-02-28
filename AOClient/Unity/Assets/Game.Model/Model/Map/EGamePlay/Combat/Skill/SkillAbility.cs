using System;
using GameUtils;
using ET;
using System.Collections.Generic;
using UnityEngine;

#if !EGAMEPLAY_EXCEL
namespace EGamePlay.Combat
{
    public partial class SkillAbility : Entity, IAbilityEntity
    {
        public CombatEntity OwnerEntity { get { return GetParent<CombatEntity>(); } set { } }
        public CombatEntity ParentEntity { get => GetParent<CombatEntity>(); }
        public bool Enable { get; set; }
        public SkillConfigObject SkillConfig { get; set; }
        public bool Spelling { get; set; }
        public GameTimer CooldownTimer { get; } = new GameTimer(1f);
        private List<StatusAbility> ChildrenStatuses { get; set; } = new List<StatusAbility>();
        public ExecutionObject ExecutionObject { get; set; }


        public override void Awake(object initData)
        {
            base.Awake(initData);
            SkillConfig = initData as SkillConfigObject;
            Name = SkillConfig.Name;
            AddComponent<AbilityEffectComponent>(SkillConfig.Effects);
            LoadExecution();
            if (SkillConfig.SkillSpellType == SkillSpellType.Passive)
            {
                TryActivateAbility();
            }
        }

        public void LoadExecution()
        {
            ET.Log.Console($"LoadExecution SkillConfig.Id={SkillConfig.Id}");
            ExecutionObject = AssetUtils.Load<ExecutionObject>($"Execution_{SkillConfig.Id}");
            ET.Log.Console($"LoadExecution {ExecutionObject.Name}");
        }

        public void TryActivateAbility()
        {
            this.ActivateAbility();
        }

        public void DeactivateAbility()
        {
            Enable = false;
        }

        public void ActivateAbility()
        {
            //base.ActivateAbility();
            FireEvent(nameof(ActivateAbility));
            //子状态效果
            if (SkillConfig.EnableChildrenStatuses)
            {
                foreach (var item in SkillConfig.ChildrenStatuses)
                {
                    var status = OwnerEntity.AttachStatus(item.StatusConfigObject);
                    status.OwnerEntity = OwnerEntity;
                    status.IsChildStatus = true;
                    status.ChildStatusData = item;
                    status.ProcessInputKVParams(item.Params);
                    status.TryActivateAbility();
                    ChildrenStatuses.Add(status);
                }
            }
        }

        public void EndAbility()
        {
            //子状态效果
            if (SkillConfig.EnableChildrenStatuses)
            {
                foreach (var item in ChildrenStatuses)
                {
                    item.EndAbility();
                }
                ChildrenStatuses.Clear();
            }
            Entity.Destroy(this);
        }

        public Entity CreateExecution()
        {
            var execution = OwnerEntity.AddChild<SkillExecution>(this);
            execution.ExecutionObject = ExecutionObject;
            execution.LoadExecutionEffects();
            this.FireEvent(nameof(CreateExecution), execution);
            if (ExecutionObject != null)
            {
                execution.AddComponent<UpdateComponent>();
            }
            return execution;
        }
    }
}
#endif