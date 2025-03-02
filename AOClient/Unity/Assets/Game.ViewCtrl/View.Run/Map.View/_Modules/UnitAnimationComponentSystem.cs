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
                self.AnimationType = AnimationType.Idle;
            }
        }

        public static async void Play(this TComp self, AnimationType animationType)
        {
            var animator = self.Parent.GetComponent<UnitViewComponent>().UnitObj.GetComponentInChildren<Animator>();
            if (animator == null)
            {
                return;
            }
            if (animationType == self.AnimationType)
            {
                return;
            }
            //Log.Debug($"Play {animationType}");
            if (animationType == AnimationType.Run)
            {
                //animator.SetFloat("Walk", 3);
                animator.SetTrigger("Run");
            }
            if (animationType == AnimationType.Idle)
            {
                //animator.SetFloat("Walk", 0);
                animator.SetTrigger("Idle");
            }
            if (animationType == AnimationType.Attack)
            {
                animator.SetTrigger("Attack");
            }
            self.AnimationType = animationType;

            if (animationType == AnimationType.Attack)
            {
                await TimeHelper.WaitAsync(1000);
                self.Play(AnimationType.Idle);
            }
        }
    }
}