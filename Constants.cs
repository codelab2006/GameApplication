namespace GameApplication
{
    public static class Constants
    {
        public const int VirtualWidth = 1920;
        public const int VirtualHeight = 1080;

        public const int UnitWidth = 16;
        public const int UnitHeight = 16;

        public const int WorldHCount = 5000; // 10000;
        public const int WorldVCount = 2500; // 5000;

        public static readonly int VirtualWorldHCount;
        public static readonly int VirtualWorldVCount;

        static Constants()
        {
            VirtualWorldHCount = VirtualWidth / UnitWidth + 1;
            VirtualWorldVCount = VirtualHeight / UnitHeight + 1;
        }

        public static void Initialize() { }
    }
}