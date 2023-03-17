using GameUtils;
using UnityEngine;

namespace EGamePlay.Combat
{
    public sealed class ConditionTimeIntervalEvent : Entity
    {
        private GameTimer IntervalTimer { get; set; }


        public override void Awake(object initData)
        {
            var time = (float)initData;
            IntervalTimer = new GameTimer(time);
            AddComponent<UpdateComponent>();
        }

        public override void Update()
        {
            if (IntervalTimer.IsRunning)
            {
                IntervalTimer.UpdateAsRepeat(Time.deltaTime, WhenInterval);
            }
        }

        private void WhenInterval()
        {
            //Log.Debug($"{GetType().Name}->WhenReceiveDamage");
            GetParent<EffectTriggerEventBind>().TriggerSelfEffectCheck();
        }
    }
}