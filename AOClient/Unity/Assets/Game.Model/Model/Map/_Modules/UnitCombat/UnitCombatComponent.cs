namespace AO
{
    using ET;
    using EGamePlay.Combat;

    public partial class UnitCombatComponent : Entity, IAwake
    {
        public CombatEntity CombatEntity { get; set; }
    }
}