namespace AO
{
    using ET;
    using Unity.Mathematics;

    public interface IMapUnit : IUnit
    {
        public float3 Position { get; set; }
    }
}