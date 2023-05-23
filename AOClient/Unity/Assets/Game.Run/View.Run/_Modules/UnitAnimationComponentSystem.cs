using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TComp = AO.UnitAnimationComponent;

namespace AO
{
    public static class UnitAnimationComponentSystem
    {
        [ObjectSystem]
        public class UnitAnimationComponentAwakeSystem : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                
            }
        }

        public static void Play(this TComp self, AnimationType animationType)
        {
            var animator = self.Parent.GetComponent<UnitViewComponent>().UnitObj.GetComponentInChildren<Animator>();
            if (animator == null)
            {
                return;
            }
            if (animationType == AnimationType.Run)
            {
                animator.SetFloat("Walk", 3);
            }
            if (animationType == AnimationType.Idle)
            {
                animator.SetFloat("Walk", 0);
            }
            if (animationType == AnimationType.Attack)
            {
                animator.Play("Attack");
            }
        }
    }
}