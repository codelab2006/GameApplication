using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public abstract class Sprite
    {
        private readonly Texture2D? _texture2D;
        private readonly Rectangle? _sourceRectangle;
        public Vector2 Position { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; } = 0;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0;

        public Sprite(Texture2D? texture2D, Rectangle? sourceRectangle)
        {
            _texture2D = texture2D;
            _sourceRectangle = sourceRectangle ?? texture2D?.Bounds;

            if (_sourceRectangle.HasValue)
                Origin = new(_sourceRectangle.Value.Width / 2, _sourceRectangle.Value.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_texture2D is not null)
                spriteBatch.Draw(_texture2D, Position, _sourceRectangle, Color, Rotation, Origin, Scale, Effects, LayerDepth);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture2D)
        {
            spriteBatch.Draw(texture2D, Position, _sourceRectangle, Color, Rotation, Origin, Scale, Effects, LayerDepth);
        }
    }
}