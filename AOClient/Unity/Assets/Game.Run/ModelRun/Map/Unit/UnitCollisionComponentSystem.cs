using System;
using ET;
using Unity.Mathematics;
using TComp = AO.UnitCollisionComponent;

namespace AO
{
    public static class UnitCollisionComponentSystem
    {
        public class UnitCollisionComponentAwakeSystem : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                var scene = self.Parent.Parent as Scene;
                foreach (IMapUnit otherUnit in scene.GetComponent<SceneUnitComponent>().GetAll())
                {
                    if (otherUnit == self.Parent)
                    {
                        continue;
                    }
                    if (self.Parent is ItemUnit itemUnit && otherUnit == itemUnit.OwnerUnit)
                    {
                        continue;
                    }
                    if (math.distance(otherUnit.Position, self.GetParent<Entity>().MapUnit().Position) < 2)
                    {
                        //self.OnEnterCollision(otherUnit);
                    }
                }
            }
        }

        public class UnitCollisionComponentUpdateSystem : UpdateSystem<TComp>
        {
            protected override void Update(TComp self)
            {
            }
        }

        public static void OnEnterCollision(this TComp self, IMapUnit otherUnit)
        {
            Log.Console($"UnitCollisionComponentSystem OnEnterCollision otherUnit={otherUnit.Entity().Id} {otherUnit.GetType().Name}");
            otherUnit.Entity().GetComponent<AttributeHPComponent>().HPValue -= 10;
        }

        public static void OnStayCollision(this TComp self, IMapUnit otherUnit)
        {
            //Log.Console($"UnitCollisionComponentSystem OnStayCollision");

        }

        public static void OnLeaveCollision(this TComp self, IMapUnit otherUnit)
        {
            Log.Console($"UnitCollisionComponentSystem OnLeaveCollision");

        }
    }
}