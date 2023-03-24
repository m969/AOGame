using ET;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using Unity.Mathematics;

namespace AO
{
    public class UnitPathMoveComponent : Entity, IAwake, IDestroy, IUpdate, IBsonIgnore
    {
        public IMapUnit Unit { get; set; }
        public List<float3> PathPoints { get; set; }
        [NotifyAOI, PropertyChanged]
        public float Speed { get; set; } = 2f;
    }
}
