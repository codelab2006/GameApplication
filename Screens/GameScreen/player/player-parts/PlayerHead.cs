using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class PlayerHead : PlayerPart
    {
        public PlayerHead() { }

        public PlayerHead(Texture2D texture2D, Vector2 position, PlayerBody playerBody) : base(texture2D, position)
        {
            Origin = new(Rectangle.Width / 2, Rectangle.Height);
            var bodyRectangle = playerBody.Rectangle;
            position.Y = bodyRectangle.Center.Y - bodyRectangle.Height / 2 + 1;
            Position = position;
        }
    }
}