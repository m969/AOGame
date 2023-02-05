namespace AO
{
    using ET;
    using Unity.Mathematics;

    public interface IUnit
    {
        public string? Name { get; set; }
        public int ConfigId { get; set; }
    }

    public enum UnitType : byte
    {
        Player = 1,
        Enemy = 2,
        Npc = 3,
    }
}