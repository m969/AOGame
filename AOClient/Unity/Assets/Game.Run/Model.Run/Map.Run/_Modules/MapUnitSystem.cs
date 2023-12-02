﻿namespace AO
{
    using AO;
    using ET;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime;
    using Unity.Mathematics;

    public static partial class MapUnitSystem
    {
        public static void SetMapUnitComponents(this IMapUnit unit)
        {
            var entity = unit.Entity();
            entity.AddComponent<UnitTranslateComponent>();
            entity.AddComponent<UnitPathMoveComponent>();
        }

        public static void AddAOI(this IMapUnit unit)
        {
#if !UNITY
            var entity = unit.Entity();
            entity.AddComponent<ET.Server.AOIEntity, int, float3>(9 * 1000, unit.Position);
#endif
        }

        public static UnitInfo CreateUnitInfo(this IMapUnit unit)
        {
            var unitInfo = new UnitInfo();
            unitInfo.UnitId = unit.Entity().Id;
            unitInfo.ConfigId = unit.ConfigId;
            unitInfo.Position = unit.Position;
            unitInfo.Name = unit.Name;
            if (unit.Entity().GetComponent<UnitPathMoveComponent>() != null && unit.Entity().GetComponent<UnitPathMoveComponent>().PathPoints != null)
            {
                unitInfo.MoveInfo = new MoveInfo();
                unitInfo.MoveInfo.Points = unit.Entity().GetComponent<UnitPathMoveComponent>().PathPoints.ToList();
                unitInfo.MoveInfo.MoveSpeed = (int)(unit.Entity().GetComponent<UnitPathMoveComponent>().Speed * 100);
            }
            if (unit is Avatar) unitInfo.Type = ((int)UnitType.Player);
            if (unit is NpcUnit) unitInfo.Type = ((int)UnitType.Enemy);
            if (unit is ItemUnit) unitInfo.Type = ((int)UnitType.ItemUnit);
            if (unit is Scene) unitInfo.Type = ((int)UnitType.Scene);
            unitInfo.ComponentInfos = new List<ComponentInfo>();
            var notifyAOIComps = unit.Entity().GetNotifyAOIComponents();
            foreach (var comp in notifyAOIComps)
            {
                if (comp is AO.IBsonIgnore)
                {
                    continue;
                }
                var compBytes = MongoHelper.Serialize(comp);
                unitInfo.ComponentInfos.Add(new ComponentInfo() { ComponentName = $"{comp.GetType().FullName}", ComponentBytes = compBytes });
            }
            return unitInfo;
        }

        public static bool CheckIsCombatUnit(this Entity unit) => unit.MapUnit().CheckIsCombatUnit();

        public static bool CheckIsCombatUnit(this IMapUnit unit)
        {
            if (unit is Avatar || unit is NpcUnit)
            {
                if (unit.Entity().GetComponent<AttributeHPComponent>().AvailableValue > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}