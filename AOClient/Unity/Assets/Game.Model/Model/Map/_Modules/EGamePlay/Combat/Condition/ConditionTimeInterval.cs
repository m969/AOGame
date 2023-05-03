using GameUtils;
using System;
using UnityEngine;

namespace EGamePlay.Combat
{
    public sealed class ConditionTimeInterval : Entity
    {
        private GameTimer IntervalTimer { get; set; }


        public override void Awake(object initData)
        {
            var time = (float)initData;
            IntervalTimer = new GameTimer(time);
        }

        public void StartListen(Action whenNoDamageInTimeCallback)
        {
            IntervalTimer.OnRepeat(whenNoDamageInTimeCallback);
            AddComponent<UpdateComponent>();
        }

        public override void Update()
        {
            //ET.Log.Console($"ConditionTimeInterval {IntervalTimer.IsRunning}");
            if (IntervalTimer.IsRunning)
            {
                IntervalTimer.UpdateAsRepeat(Time.deltaTime);
            }
        }

        //private void WhenInterval()
        //{
        //    Log.Error("ConditionTimeInterval WhenInterval");
        //}
    }
}