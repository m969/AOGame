namespace AO
{
    using AO;
    using ET;
    using Unity.Mathematics;

    public class UnitSpawnPointComponent : Entity, IAwake<float3>
    {
        public float3 SpawnPoint { get; set; }
    }
}