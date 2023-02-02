namespace AO
{
    using ET;

    public partial class LevelComponent : Entity, IAwake
    {
        [NotifyAOI]
        public int Level { get; set; }
    }
}