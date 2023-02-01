namespace AO
{
    using ET;
    using Unity.Mathematics;

    public partial class Avatar : Entity, IUnit, IAwake
    {
        [Notify]
        public string? Name { get; set; }
        [Notify]
        public int ConfigId { get; set; }
        [Notify]
        public float3 Position { get; set; }
    }
}