using Microsoft.Xna.Framework;

namespace GameApplication
{
    public static class Constants
    {
        public const int VirtualWidth = 1280; // 1920;
        public const int VirtualHeight = 720; // 1080;

        public const int UnitWidth = 20;
        public const int UnitHeight = 20;

        public const int BG_UNIT_PADDING = 6;

        public const int WorldHCount = 121; // 8400;
        public const int WorldVCount = 121; // 2400;

        public const int CollisionMargin = 20;

        public const int CollisionStep = 5;

        public const int MaxHorizontalVelocity = 800;

        public const int MaxVerticalVelocity = 800;

        public const float InitialHorizontalAcceleration = 5f;

        public const float InitialUpAcceleration = 360f;

        public const float ContinueUpAcceleration = 10f;

        public const float FrictionCoefficient = 0.25f;

        public const int PlayerWidth = 23;

        public const int PlayerHeight = 53;

        public const int PlayerRenderTarget2DHMargin = 35;

        public const int PlayerRenderTarget2DVMargin = 20;

        public static readonly Vector2 GravityAcceleration = Vector2.Zero;

        public static readonly int ADayTime = 60 * 24;

        public static readonly bool ShowPlayer = true;

        public static readonly bool RefreshBrightness = false;

        public static readonly float MinLightIntensity = 0.02f;
        static Constants()
        {
            GravityAcceleration = new Vector2(0, 10f);
        }

        public static void Initialize() { }
    }
}