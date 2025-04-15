using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class World(int aDayTime) : ILightRenderer
    {
        public Unit?[,] Units { get; private set; } = new Unit[0, 0];
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;

        private Texture2D? _texture2D;

        private readonly int _aDayTime = aDayTime;
        private float _progress = 0f;

        private readonly Texture2D _texture2DWhite = Global.GameGraphicsDevice.CreateTexture2D(1, 1);
        private readonly Texture2D _texture2DWhiteA050 = Global.GameGraphicsDevice.CreateTexture2D(1, 1);
        private readonly Texture2D _texture2DWhiteA0 = Global.GameGraphicsDevice.CreateTexture2D(1, 1, [Color.White * 0f]);

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


            Units[Units.GetLength(0) / 2 + 4, Units.GetLength(1) / 2] = new Unit(UnitFG.DIRT);

            Units[Units.GetLength(0) / 2 + 4, Units.GetLength(1) / 2 + 1] = new Unit(UnitFG.DIRT);
            Units[Units.GetLength(0) / 2 + 3, Units.GetLength(1) / 2 + 1] = new Unit(UnitFG.DIRT);

            Units[Units.GetLength(0) / 2 + 4, Units.GetLength(1) / 2 + 2] = new Unit(UnitFG.DIRT);
            Units[Units.GetLength(0) / 2 + 3, Units.GetLength(1) / 2 + 2] = new Unit(UnitFG.DIRT);
            Units[Units.GetLength(0) / 2 + 2, Units.GetLength(1) / 2 + 2] = new Unit(UnitFG.DIRT);

            Width = Units.GetLength(1) * Constants.UnitWidth;
            Height = Units.GetLength(0) * Constants.UnitHeight;

            Console.WriteLine($"World Width: {Width}, Height: {Height}");
        }

        public void LoadContent()
        {
            _texture2D = Global.Content.Load<Texture2D>("world-units");
        }

        public void SetUnitAt(int vi, int hi, Unit? unit)
        {
            Units[vi, hi] = unit;
        }

        public void Update(GameTime gameTime)
        {
            _progress += gameTime.GetElapsedSeconds() / _aDayTime;
            if (_progress > 1f) _progress -= 1f;
            var brightness = DayNightCycle.GetBrightnessAlpha(_progress);
            _texture2DWhite.SetData([Color.White * brightness]);
            _texture2DWhiteA050.SetData([Color.White * MathF.Min(0.50f, brightness)]);
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

        public void DrawLight(SpriteBatch spriteBatch, Vector2 position)
        {
            var (vFrom, vTo, hFrom, hTo) = Global.GetTargetUnitsRange(position, Constants.VirtualWidth, Constants.VirtualHeight, Constants.UnitHeight, Constants.UnitWidth, Constants.WorldVCount, Constants.WorldHCount);
            for (int i = vFrom; i < vTo; i++)
            {
                for (int j = hFrom; j < hTo; j++)
                {
                    var unit = Global.World.Units[i, j];
                    if (unit == null)
                    {
                        spriteBatch.Draw(
                            _texture2DWhite,
                            new Rectangle(j * Constants.UnitWidth, i * Constants.UnitHeight, Constants.UnitWidth, Constants.UnitHeight),
                            Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(
                            _texture2DWhiteA050,
                            new Rectangle(j * Constants.UnitWidth, i * Constants.UnitHeight, Constants.UnitWidth, Constants.UnitHeight),
                            Color.White);
                    }
                }
            }
        }
    }

    public class DayNightCycle
    {
        private static readonly Color[] DayColors =
        [
            new Color(0, 0, 80),        // 0:00
            new Color(80, 0, 150),      // 3:00
            new Color(0, 150, 255),     // 6:00
            new Color(50, 180, 255),    // 9:00
            new Color(0, 160, 255),     // 12:00
            new Color(0, 140, 240),     // 15:00
            new Color(0, 100, 200),     // 18:00
            new Color(0, 0, 120),       // 21:00
            new Color(0, 0, 80)         // 24:00
        ];

        private static readonly float[] Brightness =
        {
            0f,                         // 0:00
            0.15f,                      // 3:00
            0.85f,                      // 6:00
            1.0f,                       // 9:00
            1.0f,                       // 12:00
            1.0f,                       // 15:00
            0.85f,                      // 18:00
            0.15f,                      // 21:00
            0f                          // 24:00
        };

        public static Color GetBackgroundColor(float t)
        {
            return EvaluateCurve(t, DayColors, Color.Lerp);
        }

        public static float GetBrightnessAlpha(float t)
        {
            return EvaluateCurve(t, Brightness, MathHelper.Lerp);
        }

        private static T EvaluateCurve<T>(float t, T[] curve, Func<T, T, float, T> lerpFunc)
        {
            int count = curve.Length - 1;
            float segmentLength = 1f / count;

            int index = (int)(t / segmentLength);
            float localT = (t - (index * segmentLength)) / segmentLength;

            T from = curve[index];
            T to = curve[(index + 1) % curve.Length];

            return lerpFunc(from, to, localT);
        }
    }
}