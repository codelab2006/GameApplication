namespace GameApplication
{
    public enum UnitBG
    {
        NONE = 0,
    }

    public enum UnitFG
    {
        NONE = 0,
        DIRT = 1,
        STONE = 2,
    }

    public class Unit : IUnit
    {
        public UnitFG FG { get; set; } = UnitFG.NONE;
        public UnitBG BG { get; set; } = UnitBG.NONE;
        public bool IsStatic => FG != UnitFG.NONE;

        public Unit(UnitBG unitBG)
        {
            BG = unitBG;
        }

        public Unit(UnitFG unitFG)
        {
            FG = unitFG;
        }
    }
}