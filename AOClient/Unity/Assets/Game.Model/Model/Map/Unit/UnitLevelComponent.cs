namespace AO
{
    using ET;

    public partial class UnitLevelComponent : Entity, IAwake
    {
        [NotifyAOI]
        public int Level { get; set; }
    }
}