using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameUtils;
using ET;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 
    /// </summary>
    public class AbilityItemFollowComponent : Component
    {
        public long FollowInterval { get; set; } = 50;
        public long NextFollowTime { get; set; }


        public override void Update()
        {
            var now = TimeHelper.ClientNow();
            if (now > NextFollowTime)
            {
                NextFollowTime = now + FollowInterval;

                var abilityItem = GetEntity<AbilityItem>();
                //Log.Debug($"abilityItem {abilityItem.Position}");
                var moveComp = abilityItem.GetComponent<AbilityItemPathMoveComponent>();
                moveComp.FollowMove();
            }
        }
    }
}