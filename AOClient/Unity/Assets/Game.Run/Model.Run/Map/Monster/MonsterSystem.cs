using EGamePlay.Combat;
using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TComp = AO.Monster;

namespace AO
{
    public static partial class MonsterSystem
    {
#if UNITY
        public class AwakeSystemObject : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                var combatEntity = CombatContext.Instance.AddChild<CombatEntity>();
                combatEntity.Unit = self;
                combatEntity.Position = self.MapUnit().Position;
                self.AddComponent<UnitCombatComponent>().CombatEntity = combatEntity;
            }
        }
#endif
    }
}