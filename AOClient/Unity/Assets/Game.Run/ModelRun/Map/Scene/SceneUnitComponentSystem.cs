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
                //while (self.CollisionUnitRemove.Count > 0)
                //{
                //    self.CollisionUnitCache.Remove(self.CollisionUnitRemove.Dequeue());
                //}
                //while (self.CollisionUnitAdd.Count > 0)
                //{
                //    self.CollisionUnitCache.Add(self.CollisionUnitAdd.Dequeue());
                //}
                self.CollisionUnitCache = self.idUnits.Values.ToList();
                foreach (var collisionUnit in self.CollisionUnitCache)
                {
                    var collisionComp = collisionUnit.GetComponent<UnitCollisionComponent>();
                    if (collisionComp == null)
                    {
                        continue;
                    }
                    //var numericComp = collisionUnit.GetComponent<NumericComponent>();
                    //var type = numericComp.GetAsInt(NumericType.CollisionType);
                    //var radius = numericComp.GetAsFloat(NumericType.Radius);
                    //var length = numericComp.GetAsFloat(NumericType.Length);

                    var type = collisionComp.CollisionShape;
                    var radius = collisionComp.Radius;

                    //var aoiEntity = collisionUnit.GetComponent<AOIEntity>();
                    //while (aoiEntity.SeeUnitsRemove.Count > 0)
                    //{
                    //    aoiEntity.SeeUnitsCache.Remove(aoiEntity.SeeUnitsRemove.Dequeue());
                    //}
                    //while (aoiEntity.SeeUnitsAdd.Count > 0)
                    //{
                    //    aoiEntity.SeeUnitsCache.Add(aoiEntity.SeeUnitsAdd.Dequeue());
                    //}

                    foreach (var seeEntity in self.CollisionUnitCache)
                    {
                        var seeUnit = seeEntity;
                        if (seeUnit == collisionUnit)
                        {
                            continue;
                        }
                        if (!seeUnit.CheckIsCombatUnit())
                        {
                            continue;
                        }

                        if (type == CollisionShape.Box)
                        {
                            //var dir = seeUnit.Position - collisionUnit.Position;
                            //var dotForward = Vector3.Dot(collisionUnit.Forward.normalized, dir);
                            //var dotRight = Vector3.Dot((collisionUnit.Rotation * Vector3.right).normalized, dir);

                            //if (dotForward >= 0 && dotForward <= length && Mathf.Abs(dotRight) <= radius / 2)
                            //{
                            //    if (!collisionComp.StayUnits.Contains(seeUnit.Id))
                            //    {
                            //        //Log.Console($"{seeUnit.Position} {collisionUnit.Position} dist={dist} radius={radius}");
                            //        collisionComp.StayUnits.Add(seeUnit.Id);
                            //        collisionComp.OnEnterCollision(seeUnit);
                            //    }
                            //    collisionComp.OnStayCollision(seeUnit);
                            //}
                            //else
                            //{
                            //    if (collisionComp.StayUnits.Contains(seeUnit.Id))
                            //    {
                            //        collisionComp.StayUnits.Remove(seeUnit.Id);
                            //        collisionComp.OnLeaveCollision(seeUnit);
                            //    }
                            //}
                        }
                        else if (type == CollisionShape.Sphere)
                        {
                            var dist = math.distance(seeUnit.MapUnit().Position, collisionUnit.MapUnit().Position);
                            if (dist < radius)
                            {
                                if (!collisionComp.StayUnits.Contains(seeUnit.Id))
                                {
                                    //Log.Console($"{seeUnit.Position} {collisionUnit.Position} dist={dist} radius={radius}");
                                    collisionComp.StayUnits.Add(seeUnit.Id);
                                    collisionComp.OnEnterCollision(seeUnit.MapUnit());
                                }
                                collisionComp.OnStayCollision(seeUnit.MapUnit());
                            }
                            else
                            {
                                if (collisionComp.StayUnits.Contains(seeUnit.Id))
                                {
                                    collisionComp.StayUnits.Remove(seeUnit.Id);
                                    collisionComp.OnLeaveCollision(seeUnit.MapUnit());
                                }
                            }
                        }
                    }
                }
            }
        }

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