using Microsoft.Xna.Framework;

namespace GameApplication
{
    public static class Constants
    {
        public const int VirtualWidth = 1280;// 1920;
        public const int VirtualHeight = 720;// 1080;

        public const int UnitWidth = 16;
        public const int UnitHeight = 16;

        public const int WorldHCount = 10000;
        public const int WorldVCount = 5000;

        public const int CollisionMargin = 8;

        public const int MaxHorizontalVelocity = 300;

        public const int MaxVerticalVelocity = 300;

        public const float InitialHorizontalAcceleration = 10f;

        public const float FrictionCoefficient = 0.5f;

        public static readonly Vector2 GravityAcceleration = Vector2.Zero;

        static Constants()
        {
            GravityAcceleration = new Vector2(0, 10f);
        }

        public static void Initialize() { }
    }
}