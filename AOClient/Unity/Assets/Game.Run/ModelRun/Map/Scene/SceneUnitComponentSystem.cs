using ET;
using System.Linq;

namespace AO
{
    public static class SceneUnitComponentSystem
    {
        //[FriendClass(typeof(AOIEntity))]
        //public class MapUnitComponentUpdateSystem : UpdateSystem<MapUnitComponent>
        //{
        //    public override void Update(MapUnitComponent self)
        //    {
        //        while (self.CollisionUnitRemove.Count > 0)
        //        {
        //            self.CollisionUnitCache.Remove(self.CollisionUnitRemove.Dequeue());
        //        }
        //        while (self.CollisionUnitAdd.Count > 0)
        //        {
        //            self.CollisionUnitCache.Add(self.CollisionUnitAdd.Dequeue());
        //        }
        //        foreach (var collisionUnit in self.CollisionUnitCache)
        //        {
        //            var collisionComp = collisionUnit.GetComponent<UnitCollisionComponent>();
        //            var numericComp = collisionUnit.GetComponent<NumericComponent>();
        //            var type = numericComp.GetAsInt(NumericType.CollisionType);
        //            var radius = numericComp.GetAsFloat(NumericType.Radius);
        //            var length = numericComp.GetAsFloat(NumericType.Length);

        //            var aoiEntity = collisionUnit.GetComponent<AOIEntity>();
        //            while (aoiEntity.SeeUnitsRemove.Count > 0)
        //            {
        //                aoiEntity.SeeUnitsCache.Remove(aoiEntity.SeeUnitsRemove.Dequeue());
        //            }
        //            while (aoiEntity.SeeUnitsAdd.Count > 0)
        //            {
        //                aoiEntity.SeeUnitsCache.Add(aoiEntity.SeeUnitsAdd.Dequeue());
        //            }

        //            //var seeUnits = collisionUnit.GetSeeUnits();

        //            foreach (var seeEntity in aoiEntity.SeeUnitsCache)
        //            {
        //                var seeUnit = seeEntity.Unit;
        //                if (seeUnit == collisionUnit)
        //                {
        //                    continue;
        //                }
        //                if (!seeUnit.CheckIsCombatUnit())
        //                {
        //                    continue;
        //                }

        //                if (type == CollisionType.Box)
        //                {
        //                    var dir = seeUnit.Position - collisionUnit.Position;
        //                    var dotForward = Vector3.Dot(collisionUnit.Forward.normalized, dir);
        //                    var dotRight = Vector3.Dot((collisionUnit.Rotation * Vector3.right).normalized, dir);



        //                    if (dotForward >= 0 && dotForward <= length && Mathf.Abs(dotRight) <= radius / 2)
        //                    {
        //                        if (!collisionComp.StayUnits.Contains(seeUnit.Id))
        //                        {
        //                            //Log.Console($"{seeUnit.Position} {collisionUnit.Position} dist={dist} radius={radius}");
        //                            collisionComp.StayUnits.Add(seeUnit.Id);
        //                            collisionComp.OnEnterCollision(seeUnit);
        //                        }
        //                        collisionComp.OnStayCollision(seeUnit);
        //                    }
        //                    else
        //                    {
        //                        if (collisionComp.StayUnits.Contains(seeUnit.Id))
        //                        {
        //                            collisionComp.StayUnits.Remove(seeUnit.Id);
        //                            collisionComp.OnLeaveCollision(seeUnit);
        //                        }
        //                    }
        //                }
        //                else if (type == CollisionType.Circle)
        //                {
        //                    var dist = Vector3.Distance(seeUnit.Position, collisionUnit.Position);
        //                    if (dist < radius)
        //                    {
        //                        if (!collisionComp.StayUnits.Contains(seeUnit.Id))
        //                        {
        //                            //Log.Console($"{seeUnit.Position} {collisionUnit.Position} dist={dist} radius={radius}");
        //                            collisionComp.StayUnits.Add(seeUnit.Id);
        //                            collisionComp.OnEnterCollision(seeUnit);
        //                        }
        //                        collisionComp.OnStayCollision(seeUnit);
        //                    }
        //                    else
        //                    {
        //                        if (collisionComp.StayUnits.Contains(seeUnit.Id))
        //                        {
        //                            collisionComp.StayUnits.Remove(seeUnit.Id);
        //                            collisionComp.OnLeaveCollision(seeUnit);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public static void Add(this SceneUnitComponent self, Entity unit)
        {
            self.idUnits.Add(unit.Id, unit);
            if (unit is Avatar avatar) self.idAvatars.Add(unit.Id, avatar);
        }

        public static Entity Get(this SceneUnitComponent self, long id)
        {
            self.idUnits.TryGetValue(id, out var gamer);
            return gamer;
        }

        public static void Remove(this SceneUnitComponent self, long id)
        {
            self.idUnits.Remove(id);
            self.idAvatars.Remove(id);
        }

        public static Entity[] GetAll(this SceneUnitComponent self)
        {
            return self.idUnits.Values.ToArray();
        }

        public static Entity[] GetAllAvatars(this SceneUnitComponent self)
        {
            return self.idAvatars.Values.ToArray();
        }
    }
}