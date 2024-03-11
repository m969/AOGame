//namespace AO
//{
//    using ET;
//    using MongoDB.Bson.Serialization.Attributes;
//    using Unity.Mathematics;

//    public partial class NpcUnit : Entity, IMapUnit, IAwake
//    {
//        public string? Name { get; set; }

//        public int ConfigId { get; set; }

//        [BsonElement]
//        private float3 position;
//        [BsonIgnore]
//        public float3 Position
//        {
//            get
//            {
//                return position;
//            }
//            set
//            {
//                float3 oldPos = this.position;
//                this.position = value;
//                AOGame.Publish(new EventType.ChangePosition() { Unit = this, OldPos = oldPos });
//            }
//        }

//        [BsonIgnore]
//        public float3 Forward
//        {
//            get => math.mul(this.Rotation, math.forward());
//            set => this.Rotation = quaternion.LookRotation(value, math.up());
//        }

//        [BsonElement]
//        private quaternion rotation;

//        [BsonIgnore]
//        public quaternion Rotation
//        {
//            get => this.rotation;
//            set
//            {
//                this.rotation = value;
//                AOGame.Publish(new EventType.ChangeRotation() { Unit = this });
//            }
//        }
//    }
//}