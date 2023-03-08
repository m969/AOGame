namespace AO
{
    using AO;
    using ET;
    using Unity.Mathematics;

    public static partial class MapUnitSystem
    {
        public static async ETTask RandomMove(this IMapUnit unit)
        {
            var rx = RandomGenerator.RandFloat01() * 6 - 3;
            var rz = RandomGenerator.RandFloat01() * 6 - 3;
            var rp = new float3(unit.Position.x + rx, 0, unit.Position.z + rz);
            ET.Log.Console($"RandomMove {rp}");
            await unit.MoveToAsync(rp);
        }
    }
}