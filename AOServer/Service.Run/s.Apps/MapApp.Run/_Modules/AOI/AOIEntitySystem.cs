﻿using AO;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof(AOIEntity))]
    [FriendOf(typeof(Cell))]
    public static class AOIEntitySystem
    {
        [ObjectSystem]
        public class AwakeSystem: AwakeSystem<AOIEntity, int, float3>
        {
            protected override void Awake(AOIEntity self, int distance, float3 pos)
            {
                self.ViewDistance = distance;
                self.GetScene().GetComponent<AOIManagerComponent>().Add(self, pos.x, pos.z);
            }
        }

        [ObjectSystem]
        public class DestroySystem: DestroySystem<AOIEntity>
        {
            protected override void Destroy(AOIEntity self)
            {
                self.GetScene().GetComponent<AOIManagerComponent>()?.Remove(self);
                self.ViewDistance = 0;
                self.SeeUnits.Clear();
                self.SeePlayers.Clear();
                self.BeSeePlayers.Clear();
                self.BeSeeUnits.Clear();
                self.SubEnterCells.Clear();
                self.SubLeaveCells.Clear();
                self.Cell = null;
            }
        }
        
        public static Scene GetScene(this AOIEntity self)
        {
            return self.Parent.Parent as Scene;
        }

        // 获取在自己视野中的对象
        public static Dictionary<long, AOIEntity> GetSeeUnits(this AOIEntity self)
        {
            return self.SeeUnits;
        }

        public static Dictionary<long, AOIEntity> GetBeSeePlayers(this AOIEntity self)
        {
            return self.BeSeePlayers;
        }

        public static Dictionary<long, AOIEntity> GetSeePlayers(this AOIEntity self)
        {
            return self.SeePlayers;
        }

        // cell中的unit进入self的视野
        public static void SubEnter(this AOIEntity self, Cell cell)
        {
            cell.SubsEnterEntities.Add(self.Id, self);
            foreach (KeyValuePair<long, AOIEntity> kv in cell.AOIUnits)
            {
                if (kv.Key == self.Id)
                {
                    continue;
                }

                self.EnterSight(kv.Value);
            }
        }

        public static void UnSubEnter(this AOIEntity self, Cell cell)
        {
            cell.SubsEnterEntities.Remove(self.Id);
        }

        public static void SubLeave(this AOIEntity self, Cell cell)
        {
            cell.SubsLeaveEntities.Add(self.Id, self);
        }

        // cell中的unit离开self的视野
        public static void UnSubLeave(this AOIEntity self, Cell cell)
        {
            foreach (KeyValuePair<long, AOIEntity> kv in cell.AOIUnits)
            {
                if (kv.Key == self.Id)
                {
                    continue;
                }

                self.LeaveSight(kv.Value);
            }

            cell.SubsLeaveEntities.Remove(self.Id);
        }

        // enter进入self视野
        public static void EnterSight(this AOIEntity self, AOIEntity enter)
        {
            // 有可能之前在Enter，后来出了Enter还在LeaveCell，这样仍然没有删除，继续进来Enter，这种情况不需要处理
            if (self.SeeUnits.ContainsKey(enter.Id))
            {
                return;
            }
            
            if (!AOISeeCheckHelper.IsCanSee(self, enter))
            {
                return;
            }

            if (self.Unit.GetComponent<UnitCollisionComponent>() != null)
            {
                self.SeeUnitsAdd.Enqueue(enter);
            }
            self.SeeUnits.Add(enter.Id, enter);
            enter.BeSeeUnits.Add(self.Id, self);
            if (self.Unit.IsPlayerActor())
            {
                if (enter.Unit.IsPlayerActor())
                {
                    self.SeePlayers.Add(enter.Id, enter);
                    enter.BeSeePlayers.Add(self.Id, self);
                }
                else
                {
                    enter.BeSeePlayers.Add(self.Id, self);
                }
            }
            else
            {
                if (enter.Unit.IsPlayerActor())
                {
                    self.SeePlayers.Add(enter.Id, enter);
                }
                else
                {
                }
            }
            EventSystem.Instance.Publish(self.GetScene(), new EventType.UnitEnterSightRange() { A = self, B = enter });
        }

        // leave离开self视野
        public static void LeaveSight(this AOIEntity self, AOIEntity leave)
        {
            if (self.Id == leave.Id)
            {
                return;
            }

            if (!self.SeeUnits.ContainsKey(leave.Id))
            {
                return;
            }

            if (self.Unit.GetComponent<UnitCollisionComponent>() != null)
            {
                self.SeeUnitsRemove.Enqueue(leave);
            }
            self.SeeUnits.Remove(leave.Id);
            {
                if (leave.Unit.IsPlayerActor())
                {
                    self.SeePlayers.Remove(leave.Id);
                }
            }

            leave.BeSeeUnits.Remove(self.Id);
            {
                if (self.Unit.IsPlayerActor())
                {
                    leave.BeSeePlayers.Remove(self.Id);
                }
            }

            EventSystem.Instance.Publish(self.GetScene(), new EventType.UnitLeaveSightRange { A = self, B = leave });
        }

        /// <summary>
        /// 是否在Unit视野范围内
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static bool IsBeSee(this AOIEntity self, long unitId)
        {
            return self.BeSeePlayers.ContainsKey(unitId);
        }
    }
}