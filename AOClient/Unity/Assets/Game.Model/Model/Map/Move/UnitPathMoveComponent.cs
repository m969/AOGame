using ET;
using System.Collections.Generic;
using Unity.Mathematics;

namespace AO
{
    public class UnitPathMoveComponent : Entity, IAwake, IDestroy, IUpdate
    {
        public IMapUnit Unit { get; set; }
        public List<float3> PathPoints { get; set; }
        public float Speed { get; set; } = 2f;
    }
}
