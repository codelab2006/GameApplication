using Microsoft.Xna.Framework;

namespace GameApplication
{
    public static class GameTimeExtensions
    {
        public static float GetElapsedSeconds(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}