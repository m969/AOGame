namespace AO
{
    using ET;
    using ET.EventType;
    using EGamePlay.Combat;
    using Unity.Mathematics;
    using MongoDB.Bson.Serialization.Attributes;

    public enum ItemType
    {
        DroppedItem,
        CollectableItem,
    }

    public partial class ItemUnit : Entity, IAwake, IMapUnit
    {
        public ItemType ItemType;

        public string? Name { get; set; }

        public int ConfigId { get; set; }

        [BsonElement]
        private float3 position;
        [BsonIgnore]
        public float3 Position
        {
            get
            {
                return position;
            }
            set
            {
                float3 oldPos = this.position;
                this.position = value;
                AOGame.Publish(new ChangePosition() { Unit = this, OldPos = oldPos });
            }
        }

        [BsonIgnore]
        public float3 Forward
        {
            get => math.mul(this.Rotation, math.forward());
            set => this.Rotation = quaternion.LookRotation(value, math.up());
        }

        [BsonElement]
        private quaternion rotation;

        [BsonIgnore]
        public quaternion Rotation
        {
            get => this.rotation;
            set
            {
                this.rotation = value;
                AOGame.Publish(new ChangeRotation() { Unit = this });
            }
        }

        public Entity OwnerUnit { get; set; }
        public AbilityItem AbilityItem { get; set; }
    }
}