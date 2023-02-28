namespace AO
{
    using AO;
    using ET;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime;

    public static class IMapUnitSystem
    {
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
            }
            if (unit is Avatar) unitInfo.Type = ((int)UnitType.Player);
            if (unit is EnemyUnit) unitInfo.Type = ((int)UnitType.Enemy);
            if (unit is ItemUnit) unitInfo.Type = ((int)UnitType.ItemUnit);
            unitInfo.ComponentInfos = new List<ComponentInfo>();
            var notifyAOIComps = unit.Entity().GetNotifyAOIComponents();
            foreach (var comp in notifyAOIComps)
            {
                var compBytes = MongoHelper.Serialize(comp);
                unitInfo.ComponentInfos.Add(new ComponentInfo() { ComponentName = $"{comp.GetType().FullName}", ComponentBytes = compBytes });
            }
            return unitInfo;
        }

        public static bool CheckIsCombatUnit(this Entity unit) => unit.MapUnit().CheckIsCombatUnit();
        public static bool CheckIsCombatUnit(this IMapUnit unit)
        {
            if (unit is Avatar || unit is EnemyUnit)
            {
                if (unit.Entity().GetComponent<AttributeHPComponent>().Available_HP > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}