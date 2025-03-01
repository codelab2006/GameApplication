using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public abstract class Sprite
    {
        private readonly Texture2D? _texture2D;
        private readonly Rectangle? _sourceRectangle;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; } = 0;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0;

        public Sprite() : this(null) { }

        public Sprite(Texture2D? texture2D) : this(texture2D, null) { }

        public Sprite(int xIndex, int yIndex, int width, int height) : this(null, new Rectangle(xIndex, yIndex, width, height)) { }

        public Sprite(Texture2D? texture2D, Rectangle? sourceRectangle)
        {
            _texture2D = texture2D;
            _sourceRectangle = sourceRectangle ?? texture2D?.Bounds;

            if (_sourceRectangle.HasValue)
                Origin = new(_sourceRectangle.Value.Width / 2, _sourceRectangle.Value.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, null);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2? position)
        {
            if (_texture2D is not null)
                Draw(spriteBatch, _texture2D, position);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture2D, Vector2? position)
        {
            spriteBatch.Draw(texture2D, position ?? Position, _sourceRectangle, Color, Rotation, Origin, Scale, Effects, LayerDepth);
        }
    }
}