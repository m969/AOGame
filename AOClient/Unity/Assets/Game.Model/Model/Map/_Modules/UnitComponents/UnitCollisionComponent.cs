namespace AO
{
    using ET;
    using Sirenix.OdinInspector;
    using System.Collections.Generic;

    [LabelText("碰撞体形状")]
    public enum CollisionShape
    {
        [LabelText("圆形")]
        Sphere,
        [LabelText("矩形")]
        Box,
        [LabelText("扇形")]
        Sector,
        [LabelText("自定义")]
        Custom,
    }

    public partial class UnitCollisionComponent : Entity, IAwake, IUpdate
    {
        public HashSet<long> StayUnits { get; set; } = new HashSet<long>();
        [NotifyAOI]
        public CollisionShape CollisionShape { get; set; } = CollisionShape.Sphere;
        [NotifyAOI]
        public float Radius { get; set; } = 1f;
    }
}