namespace AO
{
    using ET;

    public partial class UnitLevelComponent : Entity, IAwake, IUnitDBComponent
    {
        [NotifyAOI, PropertyChanged]
        public int Level { get; set; }
    }
}