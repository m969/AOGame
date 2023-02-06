using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY
namespace EGamePlay.Combat
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecutionParticleEffectComponent : Component
    {
        public GameObject ParticleEffectPrefab { get; set; }
        public GameObject ParticleEffectObj { get; set; }


        public override void Awake()
        {
            Entity.OnEvent(nameof(ExecutionEffect.TriggerEffect), OnTriggerStart);
            Entity.OnEvent(nameof(ExecutionEffect.EndEffect), OnTriggerEnd);
        }

        public void OnTriggerStart(Entity entity)
        {
            //Log.Debug("ExecutionAnimationComponent OnTriggerExecutionEffect");
            //Entity.GetParent<SkillExecution>().OwnerEntity.Publish(AnimationClip);
            var q = Quaternion.Euler(Entity.GetParent<SkillExecution>().OwnerEntity.Rotation);
            ParticleEffectObj = GameObject.Instantiate(ParticleEffectPrefab, Entity.GetParent<SkillExecution>().OwnerEntity.Position, q);
        }

        public void OnTriggerEnd(Entity entity)
        {
            //Log.Debug("ExecutionAnimationComponent OnTriggerExecutionEffect");
            //Entity.GetParent<SkillExecution>().OwnerEntity.Publish(AnimationClip);
            GameObject.Destroy(ParticleEffectObj);
        }
    }
}
#endif