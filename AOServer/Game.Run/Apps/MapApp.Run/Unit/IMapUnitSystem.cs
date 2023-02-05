namespace AO
{
    using AO;
    using ET;

    public static class IMapUnitSystem
    {
        public static UnitInfo CreateUnitInfo(this IMapUnit unit)
        {
            var info = new UnitInfo();
            info.UnitId = unit.Entity().Id;
            info.ConfigId = unit.ConfigId;
            info.Position = unit.Position;
            if (unit is Avatar) info.Type = ((int)UnitType.Player);
            if (unit is EnemyUnit) info.Type = ((int)UnitType.Enemy);
            return info;
        }
    }
}