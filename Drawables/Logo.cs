using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class Logo : Sprite
    {
        public Logo(Texture2D? texture2D, Rectangle? sourceRectangle) : base(texture2D, sourceRectangle)
        {
            Position = new Vector2(Constants.VirtualWidth / 2, Constants.VirtualHeight / 2);
        }
    }
}