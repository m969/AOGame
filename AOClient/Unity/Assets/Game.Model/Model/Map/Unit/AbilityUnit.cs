namespace AO
{
    using ET;
    using EGamePlay.Combat;
    using Unity.Mathematics;

    public partial class AbilityUnit : Entity, IAwake, IMapUnit
    {
        public string? Name { get; set; }

        public int ConfigId { get; set; }

        private float3 position;
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
                AOGame.Publish(new EventType.ChangePosition() { Unit = this, OldPos = oldPos });
            }
        }

        public Entity OwnerUnit { get; set; }
        public AbilityItem AbilityItem { get; set; }
    }
}