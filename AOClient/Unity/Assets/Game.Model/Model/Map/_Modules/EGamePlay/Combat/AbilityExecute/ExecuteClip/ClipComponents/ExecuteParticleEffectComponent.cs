using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

#if UNITY
namespace EGamePlay.Combat
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecuteParticleEffectComponent : Component
    {
        public GameObject ParticleEffectPrefab { get; set; }
        public GameObject ParticleEffectObj { get; set; }


        public override void Awake()
        {
            Entity.OnEvent(nameof(ExecuteClip.TriggerEffect), OnTriggerStart);
            Entity.OnEvent(nameof(ExecuteClip.EndEffect), OnTriggerEnd);
        }

        public void OnTriggerStart(Entity entity)
        {
            //Log.Debug("ExecutionAnimationComponent OnTriggerExecutionEffect");
            //Entity.GetParent<SkillExecution>().OwnerEntity.Publish(AnimationClip);
            ParticleEffectObj = GameObject.Instantiate(ParticleEffectPrefab, Entity.GetParent<SkillExecution>().OwnerEntity.Position, quaternion.LookRotation(Entity.GetParent<SkillExecution>().OwnerEntity.Rotation, math.forward()));
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