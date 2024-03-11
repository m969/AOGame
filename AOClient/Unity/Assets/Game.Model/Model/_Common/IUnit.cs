namespace AO
{
    public interface IUnit
    {
        public string? Name { get; set; }
        public int ConfigId { get; set; }
    }

    public enum UnitType : byte
    {
        Actor = 1,
        //Enemy = 2,
        //Npc = 3,
        ItemUnit = 2,
        Scene = 3,
    }
}