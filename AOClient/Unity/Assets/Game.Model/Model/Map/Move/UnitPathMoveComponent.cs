using ET;
using Unity.Mathematics;

namespace AO
{
    public class UnitPathMoveComponent : Entity, IAwake, IDestroy, IUpdate
    {
        public IMapUnit Unit { get; set; }
        public float3[] PathPoints { get; set; }
        public float Speed { get; set; } = 2f;
    }
}
