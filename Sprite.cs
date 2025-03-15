using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public abstract class Sprite
    {
        private readonly Texture2D? _texture2D;
        private readonly Rectangle _sourceRectangle = Rectangle.Empty;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; private set; } = 0;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; } = 0;
        public Rectangle Rectangle => GetRectangleByPosition(Position);

        public Sprite() : this(null) { }

        public Sprite(Texture2D? texture2D) : this(texture2D, null) { }

        public Sprite(Texture2D? texture2D, Rectangle? sourceRectangle)
        {
            _texture2D = texture2D;
            _sourceRectangle = sourceRectangle ?? texture2D?.Bounds ?? Rectangle.Empty;
            Origin = new(_sourceRectangle.Width / 2, _sourceRectangle.Height / 2);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, null);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2? position)
        {
            if (_texture2D != null)
                Draw(spriteBatch, _texture2D, position);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture2D, Vector2? position)
        {
            spriteBatch.Draw(texture2D, position ?? Position, _sourceRectangle, Color, Rotation, Origin, Scale, Effects, LayerDepth);
        }

        public Rectangle GetRectangleByPosition(Vector2 position)
        {
            return new Rectangle(
                (int)(position.X - Origin.X * Scale.X),
                (int)(position.Y - Origin.Y * Scale.Y),
                (int)(_sourceRectangle.Width * Scale.X),
                (int)(_sourceRectangle.Height * Scale.Y)
            );
        }

        public CollisionRectangle GetCollisionRectangleByPosition(Vector2 position)
        {
            return new CollisionRectangle(
                position.X - Origin.X * Scale.X,
                position.Y - Origin.Y * Scale.Y,
                _sourceRectangle.Width * Scale.X,
                _sourceRectangle.Height * Scale.Y
            );
        }
    }
}