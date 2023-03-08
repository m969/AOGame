namespace AO
{
    using AO;
    using ET;
    using EGamePlay.Combat;
    using EGamePlay;
    using TComp = AO.UnitSpawnPointComponent;
    using ET.Server;
    using Unity.Mathematics;
    using SharpCompress.Common;

    public static partial class UnitSpawnPointComponentSystem
    {
        [ObjectSystem]
        public class AwakeHandler : AwakeSystem<TComp, float3>
        {
            protected override void Awake(TComp self, float3 point)
            {
                self.SpawnPoint = point;
            }
        }

        public static float3 GetSpawnPoint(this IMapUnit self)
        {
            return self.Entity().GetComponent<UnitSpawnPointComponent>().SpawnPoint;
        }
    }
}