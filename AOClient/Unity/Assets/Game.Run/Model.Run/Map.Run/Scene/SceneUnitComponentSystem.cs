using ET;
using System.Linq;
using Unity.Mathematics;
using TComp = AO.SceneUnitComponent;

namespace AO
{
    public static class SceneUnitComponentSystem
    {
        public class UpdateSystemObject : UpdateSystem<TComp>
        {
            protected override void Update(TComp self)
            {
#if !UNITY
                while (self.CollisionUnitRemove.Count > 0)
                {
                    self.CollisionUnitCache.Remove(self.CollisionUnitRemove.Dequeue());
                }
                while (self.CollisionUnitAdd.Count > 0)
                {
                    self.CollisionUnitCache.Add(self.CollisionUnitAdd.Dequeue());
                }
                
                foreach (var collisionEntity in self.CollisionUnitCache)
                {
                    var collisionUnit = collisionEntity.MapUnit();
                    var collisionComp = collisionEntity.GetComponent<UnitCollisionComponent>();
                    if (collisionComp == null)
                    {
                        continue;
                    }

                    var type = collisionComp.CollisionShape;
                    var radius = collisionComp.Radius;
                    var length = collisionComp.Radius;

                    var aoiEntity = collisionEntity.GetComponent<ET.Server.AOIEntity>();
                    while (aoiEntity.SeeUnitsRemove.Count > 0)
                    {
                        aoiEntity.SeeUnitsCache.Remove(aoiEntity.SeeUnitsRemove.Dequeue());
                    }
                    while (aoiEntity.SeeUnitsAdd.Count > 0)
                    {
                        aoiEntity.SeeUnitsCache.Add(aoiEntity.SeeUnitsAdd.Dequeue());
                    }

                    foreach (var seeEntity in aoiEntity.SeeUnitsCache)
                    {
                        var seeUnit = seeEntity.Unit.MapUnit();
                        if (seeUnit == collisionUnit)
                        {
                            continue;
                        }
                        if (!seeUnit.CheckIsCombatUnit())
                        {
                            continue;
                        }

                        var seeUnitId = seeUnit.Entity().Id;
                        if (type == CollisionShape.Box)
                        {
                            var dir = seeUnit.Position - collisionUnit.Position;
                            var dotForward = math.dot(math.normalize(collisionUnit.Forward), dir);
                            var dotRight = math.dot(math.normalize(math.mul(collisionUnit.Rotation, new float3(1, 0, 0))), dir);

                            if (dotForward >= 0 && dotForward <= length && math.abs(dotRight) <= radius / 2)
                            {
                                if (!collisionComp.StayUnits.Contains(seeUnitId))
                                {
                                    //Log.Console($"{seeUnit.Position} {collisionUnit.Position} dist={dist} radius={radius}");
                                    collisionComp.StayUnits.Add(seeUnitId);
                                    collisionComp.OnEnterCollision(seeUnit);
                                }
                                collisionComp.OnStayCollision(seeUnit);
                            }
                            else
                            {
                                if (collisionComp.StayUnits.Contains(seeUnitId))
                                {
                                    collisionComp.StayUnits.Remove(seeUnitId);
                                    collisionComp.OnLeaveCollision(seeUnit);
                                }
                            }
                        }
                        else if (type == CollisionShape.Sphere)
                        {
                            var dist = math.distance(seeUnit.Position, collisionUnit.Position);
                            if (dist < radius)
                            {
                                if (!collisionComp.StayUnits.Contains(seeUnitId))
                                {
                                    //Log.Console($"{seeUnit.Position} {collisionUnit.Position} dist={dist} radius={radius}");
                                    collisionComp.StayUnits.Add(seeUnitId);
                                    collisionComp.OnEnterCollision(seeUnit);
                                }
                                collisionComp.OnStayCollision(seeUnit);
                            }
                            else
                            {
                                if (collisionComp.StayUnits.Contains(seeUnitId))
                                {
                                    collisionComp.StayUnits.Remove(seeUnitId);
                                    collisionComp.OnLeaveCollision(seeUnit);
                                }
                            }
                        }
                    }
                }
#endif
            }
        }

        public static void Add(this SceneUnitComponent self, Entity unit)
        {
            self.idUnits.Add(unit.Id, unit);
            if (unit is Actor avatar) self.idAvatars.Add(unit.Id, avatar);

            unit.MapUnit().AddAOI();

            if (unit.GetComponent<UnitCollisionComponent>() != null)
            {
                self.CollisionUnitAdd.Enqueue(unit);
            }
        }

        public static Entity Get(this SceneUnitComponent self, long id)
        {
            self.idUnits.TryGetValue(id, out var gamer);
            return gamer;
        }

        public static void Remove(this SceneUnitComponent self, long id)
        {
            if (!self.idUnits.TryGetValue(id, out Entity entity))
            {
                return;
            }

            self.idUnits.Remove(id);
            self.idAvatars.Remove(id);

            if (entity.GetComponent<UnitCollisionComponent>() != null)
            {
                self.CollisionUnitRemove.Enqueue(entity);
            }
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