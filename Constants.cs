using Microsoft.Xna.Framework;

namespace GameApplication
{
    public static class Constants
    {
        public const int VirtualWidth = 1280;// 1920;
        public const int VirtualHeight = 720;// 1080;

        public const int UnitWidth = 16;
        public const int UnitHeight = 16;

        public const int WorldHCount = 100; // 10000;
        public const int WorldVCount = 100; // 5000;

        public const int CollisionMargin = 8;

        public const int MaxHorizontalVelocity = 8;

        public const int MaxVerticalVelocity = 8;

        public const float InitialHorizontalAcceleration = 4;

        public static readonly Vector2 GravityAcceleration = Vector2.Zero;

        static Constants()
        {
            GravityAcceleration = new Vector2(0, 6f);
        }

        public static void Initialize() { }
    }
}