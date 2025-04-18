namespace AO
{
    using ET;
    using ET.EventType;
    using MongoDB.Bson.Serialization.Attributes;
    using Unity.Mathematics;

    public enum ActorType
    {
        Player,
        NonPlayer,/// NPC（Non Player Character）非玩家单位的统称
    }

    public partial class Actor : Entity, IMapUnit, IAwake
    {
        public ActorType ActorType { get; set; }

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

#if !UNITY
        public ActorClient.ClientCall ClientCall => GetComponent<ActorClient>().Client;
#else
        /// <summary>
        /// 本地主玩家
        /// </summary>
        public static Actor Main { get; set; }
        ///// <summary>
        ///// 本地主场景
        ///// </summary>
        //public static Scene CurrentScene { get; set; }
#endif
    }
}