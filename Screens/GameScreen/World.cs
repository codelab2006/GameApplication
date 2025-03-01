using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class World
    {
        private Unit[,] _world = new Unit[0, 0];

        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;

        private Texture2D? _texture2D;

        public void Initialize()
        {
            _world = new Unit[Constants.WorldVCount, Constants.WorldHCount];
            for (int i = 0; i != _world.GetLength(0); i++)
            {
                for (int j = 0; j != _world.GetLength(1); j++)
                {
                    _world[i, j] = new Unit();
                }
            }
            Width = _world.GetLength(1) * Constants.UnitWidth;
            Height = _world.GetLength(0) * Constants.UnitHeight;
        }

        public void LoadContent()
        {
            _texture2D = Global.Content.Load<Texture2D>("world_units");
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (_texture2D is null) return;

            var (vFrom, vTo, hFrom, hTo) = Global.GetDrawableRange(position);

            for (int i = vFrom; i != vTo; i++)
            {
                for (int j = hFrom; j != hTo; j++)
                {
                    var unit = _world[i, j];
                    if (unit is not null)
                    {
                        spriteBatch.Draw(_texture2D, new Vector2(j * Constants.UnitWidth, i * Constants.UnitHeight), new Rectangle((int)unit.BG * Constants.UnitWidth, 0, Constants.UnitWidth, Constants.UnitHeight), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    }
                }
            }
        }
    }
}