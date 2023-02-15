using EGamePlay.Combat;
using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TComp = AO.Avatar;

namespace AO
{
    public static class AvatarSystem
    {
        [ObjectSystem]
        public class AvatarAwakeSystem : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                var combatEntity = CombatContext.Instance.AddChild<CombatEntity>();
                combatEntity.Unit = self;
                combatEntity.Position = self.MapUnit().Position;
                self.AddComponent<UnitCombatComponent>().CombatEntity = combatEntity;
            }
        }

        public static Scene GetScene(this Avatar avatar)
        {
            return avatar.GetParent<Scene>();
        }
    }
}