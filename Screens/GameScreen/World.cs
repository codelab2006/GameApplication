using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class World
    {
        public Unit[,] Units { get; private set; } = new Unit[0, 0];
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;

        private Texture2D? _texture2D;

        public void Initialize()
        {
            Units = new Unit[Constants.WorldVCount, Constants.WorldHCount];
            for (int i = Units.GetLength(0) / 2 + 5; i != Units.GetLength(0); i++)
            {
                for (int j = 0; j != Units.GetLength(1); j++)
                {
                    Units[i, j] = new Unit(UnitFG.DIRT);
                }
            }

            for (int i = Units.GetLength(0) / 2 + 4; i < Units.GetLength(0) / 2 + 5; i++)
            {
                Units[i, Units.GetLength(1) / 2] = new Unit(UnitFG.DIRT);
            }

            Width = Units.GetLength(1) * Constants.UnitWidth;
            Height = Units.GetLength(0) * Constants.UnitHeight;
        }

        public void LoadContent()
        {
            _texture2D = Global.Content.Load<Texture2D>("world-units");
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(spriteBatch, position, null);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, (int vTFrom, int vTTo, int vBFrom, int vBTo, int hLFrom, int hLTo, int hRFrom, int hRTo)? highlightRange)
        {
            if (_texture2D == null) return;

            var (vFrom, vTo, hFrom, hTo) = Global.GetTargetUnitsRange(position, Constants.VirtualWidth, Constants.VirtualHeight, Constants.UnitHeight, Constants.UnitWidth, Constants.WorldVCount, Constants.WorldHCount);
            for (int i = vFrom; i < vTo; i++)
            {
                for (int j = hFrom; j < hTo; j++)
                {
                    var unit = Units[i, j];
                    if (unit != null)
                    {
                        Color color = Color.White;
                        if (highlightRange.HasValue &&
                            ((i >= highlightRange.Value.vTFrom && i < highlightRange.Value.vBTo && ((j >= highlightRange.Value.hLFrom && j < highlightRange.Value.hLTo) || (j >= highlightRange.Value.hRFrom && j < highlightRange.Value.hRTo))) ||
                            (j >= highlightRange.Value.hLFrom && j < highlightRange.Value.hRTo && ((i >= highlightRange.Value.vTFrom && i < highlightRange.Value.vTTo) || (i >= highlightRange.Value.vBFrom && i < highlightRange.Value.vBTo))))
                        )
                            color = Color.Blue;

                        spriteBatch.Draw(_texture2D, new Vector2(j * Constants.UnitWidth, i * Constants.UnitHeight), new Rectangle((int)unit.FG * Constants.UnitWidth, 0, Constants.UnitWidth, Constants.UnitHeight), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    }
                }
            }
        }
    }
}