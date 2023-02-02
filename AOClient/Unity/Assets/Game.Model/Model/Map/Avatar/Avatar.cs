namespace AO
{
    using ET;
    using Unity.Mathematics;

    public partial class Avatar : Entity, IMapUnit, IAwake
    {
        public string? Name { get; set; }

        public int ConfigId { get; set; }

        public float3 Position { get; set; }

#if UNITY
        /// <summary>
        /// 本地主玩家
        /// </summary>
        public static Avatar Main { get; set; }
#endif
    }
}