using EGamePlay.Combat;
using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TComp = AO.EnemyUnit;

namespace AO
{
    public static class EnemyUnitSystem
    {
        [ObjectSystem]
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
    }
}