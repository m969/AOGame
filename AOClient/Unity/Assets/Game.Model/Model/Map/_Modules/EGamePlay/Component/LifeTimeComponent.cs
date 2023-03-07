using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;

namespace EGamePlay
{
    /// <summary>
    /// 生命周期组件
    /// </summary>
    public class LifeTimeComponent : Component
    {
        public override bool DefaultEnable { get; set; } = true;
        public GameTimer LifeTimer { get; set; }


        public override void Awake(object initData)
        {
            //Log.Debug($"LifeTimeComponent Awake {initData}");
            LifeTimer = new GameTimer((float)initData);
        }

        public override void Update()
        {
            if (LifeTimer.IsRunning)
            {
                LifeTimer.UpdateAsFinish(Time.deltaTime, DestroyEntity);
                //Log.Debug($"LifeTimeComponent Update {LifeTimer.Time}");
            }
        }

        private void DestroyEntity()
        {
            //Log.Debug($"LifeTimeComponent DestroyEntity {LifeTimer.Time}");
            Entity.Destroy(Entity);
        }
    }
}