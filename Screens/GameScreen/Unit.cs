namespace GameApplication
{
    public enum UnitBG
    {
        NONE = 0,
        WOOD = 1,
        STONE = 2,
    }

    public enum UnitFG
    {
        NONE = 0,
        DIRT = 1,
        STONE = 2,
    }

    public class Unit : IUnit
    {
        public int Vi { get; set; } = 0;
        public int Hi { get; set; } = 0;
        public UnitFG FG { get; set; } = UnitFG.NONE;
        public UnitBG BG { get; set; } = UnitBG.NONE;
        public bool IsG => BG != UnitBG.NONE || FG != UnitFG.NONE;
        public bool IsStatic => FG != UnitFG.NONE;
        public float Intensity { get; set; } = 0;
        public float LightDecay => FG != UnitFG.NONE ? 0.55f : BG != UnitBG.NONE ? 0.85f : 0f;


        public Unit(UnitFG unitFG, int vi, int hi)
        {
            FG = unitFG;

            Vi = vi;
            Hi = hi;
        }

        public Unit(UnitBG unitBG, int vi, int hi)
        {
            BG = unitBG;

            Vi = vi;
            Hi = hi;
        }

        public Unit(UnitFG unitFG, UnitBG unitBG, int vi, int hi)
        {
            FG = unitFG;
            BG = unitBG;

            Vi = vi;
            Hi = hi;
        }
    }
}