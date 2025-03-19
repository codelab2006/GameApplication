using Microsoft.Xna.Framework;

namespace GameApplication
{
    public static class Constants
    {
        public const int VirtualWidth = 1280;// 1920;
        public const int VirtualHeight = 720;// 1080;

        public const int UnitWidth = 16;
        public const int UnitHeight = 16;

        public const int WorldHCount = 1000;
        public const int WorldVCount = 20;

        public const int CollisionMargin = 16;

        public const int CollisionStep = 8;

        public const int MaxHorizontalVelocity = 512;

        public const int MaxVerticalVelocity = 512;

        public const float InitialHorizontalAcceleration = 4f;

        public const float InitialUpAcceleration = 280f;

        public const float FrictionCoefficient = 0.25f;

        public const int PlayerWidth = 23;

        public const int PlayerHeight = 52;

        public const int PlayerRenderTarget2DHMargin = 35;

        public const int PlayerRenderTarget2DVMargin = 20;

        public static readonly Vector2 GravityAcceleration = Vector2.Zero;

        static Constants()
        {
            GravityAcceleration = new Vector2(0, 8f);
        }

        public static void Initialize() { }
    }
}