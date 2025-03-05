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
        public Rectangle Rectangle => new(
            (int)(Position.X - Origin.X * Scale.X),
            (int)(Position.Y - Origin.Y * Scale.Y),
            (int)(_sourceRectangle.Width * Scale.X),
            (int)(_sourceRectangle.Height * Scale.Y)
        );
        public Rectangle RectangleWithMargin
        {
            get
            {
                var rectangle = Rectangle;
                rectangle.X -= Constants.CollisionMargin;
                rectangle.Y -= Constants.CollisionMargin;
                rectangle.Width += Constants.CollisionMargin * 2;
                rectangle.Height += Constants.CollisionMargin * 2;

                return rectangle;
            }
        }

        public Sprite() : this(null) { }

        public Sprite(Texture2D? texture2D) : this(texture2D, null) { }

        public Sprite(int xIndex, int yIndex, int width, int height) : this(null, new Rectangle(xIndex * width, yIndex * height, width, height)) { }

        public Sprite(Texture2D? texture2D, Rectangle? sourceRectangle)
        {
            _texture2D = texture2D;
            _sourceRectangle = sourceRectangle ?? texture2D?.Bounds ?? Rectangle.Empty;
            Origin = new(_sourceRectangle.Width / 2, _sourceRectangle.Height / 2);
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