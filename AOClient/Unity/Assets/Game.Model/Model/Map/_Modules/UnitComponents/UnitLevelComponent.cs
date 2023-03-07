namespace AO
{
    using ET;

    public partial class UnitLevelComponent : Entity, IAwake
    {
        [NotifyAOI, PropertyChanged]
        public int Level { get; set; }
    }
}