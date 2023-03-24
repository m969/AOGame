namespace AO
{
    using ET;
    using GameUtils;

    public partial class UnitLifeTimeComponent : Entity, IAwake<float>, IUpdate, IDestroy, IBsonIgnore
    {
        public GameTimer LifeTimer { get; set; }
    }
}