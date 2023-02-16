namespace AO
{
    using ET;
    using GameUtils;

    public partial class UnitLifeTimeComponent : Entity, IAwake<float>, IUpdate, IDestroy
    {
        [NotifyAOI]
        public int Level { get; set; }
        public GameTimer LifeTimer { get; set; }
    }
}