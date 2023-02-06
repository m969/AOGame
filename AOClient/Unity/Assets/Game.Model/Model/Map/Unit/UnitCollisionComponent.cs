namespace AO
{
    using ET;
    using Sirenix.OdinInspector;

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

    public partial class UnitCollisionComponent : Entity, IAwake
    {
        public CollisionShape CollisionShape { get; set; }
        public float Radius { get; set; }
    }
}