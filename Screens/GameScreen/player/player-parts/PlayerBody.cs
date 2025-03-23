using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class PlayerBody : PlayerPart
    {
        public PlayerBody() { }

        public PlayerBody(Texture2D texture2D, Vector2 position) : base(texture2D, position)
        {
            position.Y += 1;
            Position = position;
        }
    }
}