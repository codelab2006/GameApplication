namespace GameApplication
{
    public enum UnitFG
    {
        NONE = 1,
    }

    public enum UnitBG
    {
        NONE = 1,
    }

    public class Unit
    {
        public UnitFG FG { get; set; } = UnitFG.NONE;
        public UnitBG BG { get; set; } = UnitBG.NONE;
    }
}