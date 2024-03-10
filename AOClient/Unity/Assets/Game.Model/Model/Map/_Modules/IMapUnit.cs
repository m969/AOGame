namespace AO
{
    using Unity.Mathematics;

    public interface IMapUnit
    {
        public string? Name { get; set; }
        public int ConfigId { get; set; }
        public float3 Position { get; set; }
        public float3 Forward { get; set; }
        public quaternion Rotation { get; set; }
    }
}