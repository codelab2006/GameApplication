using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class World(int aDayTime) : ILightRenderer
    {
        public Unit?[,] Units { get; private set; } = new Unit[0, 0];
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;

        private Texture2D? _texture2DBGUnits;
        private Texture2D? _texture2DFGUnits;

        private readonly int _aDayTime = aDayTime;
        private float _progress = 0f;
        private float _brightness = 0f;

        private readonly Texture2D _texture2DWhite = Global.GameGraphicsDevice.CreateTexture2D(1, 1, [Color.White]);

        public void Initialize()
        {
            Units = new Unit[Constants.WorldVCount, Constants.WorldHCount];
            for (int i = Units.GetLength(0) / 4; i != Units.GetLength(0) / 3; i++)
            {
                for (int j = 0; j != Units.GetLength(1); j++)
                {
                    Units[i, j] = new Unit(UnitBG.WOOD, i, j);
                }
            }
            for (int i = Units.GetLength(0) / 3; i != Units.GetLength(0); i++)
            {
                for (int j = 0; j != Units.GetLength(1); j++)
                {
                    Units[i, j] = new Unit(UnitFG.DIRT, i, j);
                }
            }

            Width = Units.GetLength(1) * Constants.UnitWidth;
            Height = Units.GetLength(0) * Constants.UnitHeight;

            Console.WriteLine($"World Width: {Width}, Height: {Height}");

            InitializeWorldIntensity();

            FloodFillWorldBGIntensity();
            FloodFillWorldFGIntensity();
        }

        private void InitializeWorldIntensity()
        {
            int worldVCount = Units.GetLength(0);
            int worldHCount = Units.GetLength(1);
            for (int i = 0; i < worldVCount; i++)
            {
                for (int j = 0; j < worldHCount; j++)
                {
                    var unit = Units[i, j];
                    if (unit == null) continue;

                    if ((i > 0 && Units[i - 1, j] == null) ||
                        (i < Units.GetLength(0) - 1 && Units[i + 1, j] == null) ||
                        (j > 0 && Units[i, j - 1] == null) ||
                        (j < Units.GetLength(1) - 1 && Units[i, j + 1] == null))
                    {
                        if (unit.BG != UnitBG.NONE) unit.BGIntensity = 1;
                        if (unit.FG != UnitFG.NONE) unit.FGIntensity = 1;
                    }
                }
            }
        }

        private void FloodFillWorldBGIntensity()
        {
            int worldVCount = Units.GetLength(0);
            int worldHCount = Units.GetLength(1);
            for (int i = 0; i < worldVCount; i++)
            {
                for (int j = 0; j < worldHCount; j++)
                {
                    FloodFillBGIntensity(i, j);
                }
            }
        }

        private void FloodFillBGIntensity(int i, int j)
        {
            int worldVCount = Units.GetLength(0);
            int worldHCount = Units.GetLength(1);
            if (i < 0 || i >= worldVCount || j < 0 || j >= worldHCount) return;
            var unit = Units[i, j];
            if (unit != null && unit.BG != UnitBG.NONE && unit.BGIntensity > 0)
            {
                Queue<Unit> queue = new();
                queue.Enqueue(unit);
                int[] dy = [-1, 1, 0, 0, -1, -1, 1, 1];
                int[] dx = [0, 0, -1, 1, -1, 1, -1, 1];
                while (queue.Count > 0)
                {
                    var u = queue.Dequeue();
                    for (int d = 0; d < 8; d++)
                    {
                        int ni = u.Vi + dy[d];
                        int nj = u.Hi + dx[d];
                        if (ni >= 0 && ni < worldVCount && nj >= 0 && nj < worldHCount)
                        {
                            var neighbor = Units[ni, nj];
                            float nextIntensity = MathHelper.Max(u.BGIntensity - Constants.BGIntensityDecay, 0);
                            if (nextIntensity >= 0)
                            {
                                if (neighbor != null && neighbor.BG != UnitBG.NONE)
                                {
                                    if (neighbor.BGIntensity < nextIntensity)
                                    {
                                        neighbor.BGIntensity = nextIntensity;
                                        if (nextIntensity > 0)
                                            queue.Enqueue(neighbor);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FloodFillWorldFGIntensity()
        {
            int worldVCount = Units.GetLength(0);
            int worldHCount = Units.GetLength(1);
            for (int i = 0; i < worldVCount; i++)
            {
                for (int j = 0; j < worldHCount; j++)
                {
                    FloodFillFGIntensity(i, j);
                }
            }
        }

        private void FloodFillFGIntensity(int i, int j)
        {
            int worldVCount = Units.GetLength(0);
            int worldHCount = Units.GetLength(1);
            if (i < 0 || i >= worldVCount || j < 0 || j >= worldHCount) return;
            var unit = Units[i, j];
            if (unit != null && unit.FG != UnitFG.NONE && unit.FGIntensity > 0)
            {
                Queue<Unit> queue = new();
                queue.Enqueue(unit);
                int[] dy = [-1, 1, 0, 0, -1, -1, 1, 1];
                int[] dx = [0, 0, -1, 1, -1, 1, -1, 1];
                while (queue.Count > 0)
                {
                    var u = queue.Dequeue();
                    for (int d = 0; d < 8; d++)
                    {
                        int ni = u.Vi + dy[d];
                        int nj = u.Hi + dx[d];
                        if (ni >= 0 && ni < worldVCount && nj >= 0 && nj < worldHCount)
                        {
                            var neighbor = Units[ni, nj];
                            float nextIntensity = MathHelper.Max(u.FGIntensity - Constants.FGIntensityDecay, 0);
                            if (nextIntensity >= 0)
                            {
                                if (neighbor != null && neighbor.FG != UnitFG.NONE)
                                {
                                    if (neighbor.FGIntensity < nextIntensity)
                                    {
                                        neighbor.FGIntensity = nextIntensity;
                                        if (nextIntensity > 0)
                                            queue.Enqueue(neighbor);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void LoadContent()
        {
            _texture2DBGUnits = Global.Content.Load<Texture2D>("world-bg-units");
            _texture2DFGUnits = Global.Content.Load<Texture2D>("world-fg-units");
        }

        public void SetUnitAt(int vi, int hi, Unit? unit)
        {
            Units[vi, hi] = unit;

            if (unit != null)
            {
                FloodFillFGIntensityWhenFGAdded(unit);
                FloodFillBGIntensityWhenBGAdded(unit);
            }
            else
            {
                FloodFillFGIntensityWhenFGRemoved(vi, hi);
                FloodFillBGIntensityWhenBGRemoved(vi, hi);
            }
        }

        private void FloodFillFGIntensityWhenFGAdded(Unit unit)
        {
            int iStart = MathHelper.Max(unit.Vi - 5, 0);
            int iEnd = MathHelper.Min(unit.Vi + 5, Units.GetLength(0));
            int jStart = MathHelper.Max(unit.Hi - 5, 0);
            int jEnd = MathHelper.Min(unit.Hi + 5, Units.GetLength(1));
            for (int i = iStart; i < iEnd; i++)
                for (int j = jStart; j < jEnd; j++)
                {
                    var u = Units[i, j];
                    if (u != null && u.FG != UnitFG.NONE) u.FGIntensity = 0;
                }
            for (int i = iStart; i < iEnd; i++)
                for (int j = jStart; j < jEnd; j++)
                    InitializeFGIntensity(i, j);

            for (int i = MathHelper.Max(unit.Vi - 10, 0); i < MathHelper.Min(unit.Vi + 10, Units.GetLength(0)); i++)
                for (int j = MathHelper.Max(unit.Hi - 10, 0); j < MathHelper.Min(unit.Hi + 10, Units.GetLength(1)); j++)
                    FloodFillFGIntensity(i, j);
        }

        private void FloodFillBGIntensityWhenBGAdded(Unit unit)
        {
            int iStart = MathHelper.Max(unit.Vi - 10, 0);
            int iEnd = MathHelper.Min(unit.Vi + 10, Units.GetLength(0));
            int jStart = MathHelper.Max(unit.Hi - 10, 0);
            int jEnd = MathHelper.Min(unit.Hi + 10, Units.GetLength(1));
            for (int i = iStart; i < iEnd; i++)
                for (int j = jStart; j < jEnd; j++)
                {
                    var u = Units[i, j];
                    if (u != null && u.BG != UnitBG.NONE) u.BGIntensity = 0;
                }
            for (int i = iStart; i < iEnd; i++)
                for (int j = jStart; j < jEnd; j++)
                    InitializeBGIntensity(i, j);

            for (int i = MathHelper.Max(unit.Vi - 20, 0); i < MathHelper.Min(unit.Vi + 20, Units.GetLength(0)); i++)
                for (int j = MathHelper.Max(unit.Hi - 20, 0); j < MathHelper.Min(unit.Hi + 20, Units.GetLength(1)); j++)
                    FloodFillBGIntensity(i, j);
        }

        private void FloodFillFGIntensityWhenFGRemoved(int vi, int hi)
        {
            int[] dy = [-1, 1, 0, 0];
            int[] dx = [0, 0, -1, 1];
            for (int d = 0; d < 4; d++)
            {
                int ni = vi + dy[d];
                int nj = hi + dx[d];
                InitializeFGIntensity(ni, nj);
            }

            for (int d = 0; d < 4; d++)
            {
                int ni = vi + dy[d];
                int nj = hi + dx[d];
                FloodFillFGIntensity(ni, nj);
            }
        }

        private void FloodFillBGIntensityWhenBGRemoved(int vi, int hi)
        {
            int[] dy = [-1, 1, 0, 0];
            int[] dx = [0, 0, -1, 1];
            for (int d = 0; d < 4; d++)
            {
                int ni = vi + dy[d];
                int nj = hi + dx[d];
                InitializeBGIntensity(ni, nj);
            }

            for (int d = 0; d < 4; d++)
            {
                int ni = vi + dy[d];
                int nj = hi + dx[d];
                FloodFillBGIntensity(ni, nj);
            }
        }

        private void InitializeBGIntensity(int i, int j)
        {
            if (i < 0 || i >= Units.GetLength(0) || j < 0 || j >= Units.GetLength(1)) return;
            var unit = Units[i, j];
            if (unit == null || unit.BG == UnitBG.NONE) return;

            bool hasNullNeighbor =
                (i > 0 && Units[i - 1, j] == null) ||
                (i < Units.GetLength(0) - 1 && Units[i + 1, j] == null) ||
                (j > 0 && Units[i, j - 1] == null) ||
                (j < Units.GetLength(1) - 1 && Units[i, j + 1] == null);

            if (hasNullNeighbor)
            {
                unit.BGIntensity = 1;
            }
            return;
        }

        private void InitializeFGIntensity(int i, int j)
        {
            if (i < 0 || i >= Units.GetLength(0) || j < 0 || j >= Units.GetLength(1)) return;
            var unit = Units[i, j];
            if (unit == null || unit.FG == UnitFG.NONE) return;

            bool hasNullNeighbor =
                (i > 0 && Units[i - 1, j] == null) ||
                (i < Units.GetLength(0) - 1 && Units[i + 1, j] == null) ||
                (j > 0 && Units[i, j - 1] == null) ||
                (j < Units.GetLength(1) - 1 && Units[i, j + 1] == null);

            if (hasNullNeighbor)
            {
                unit.FGIntensity = 1;
            }
            return;
        }

        public void Update(GameTime gameTime)
        {
            _progress += gameTime.GetElapsedSeconds() / _aDayTime;
            if (_progress > 1f) _progress -= 1f;
            _brightness = DayNightCycle.GetBrightnessAlpha(_progress);
            _brightness = 1;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(spriteBatch, position, null);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, (int vTFrom, int vTTo, int vBFrom, int vBTo, int hLFrom, int hLTo, int hRFrom, int hRTo)? highlightRange)
        {
            if (_texture2DBGUnits == null || _texture2DFGUnits == null) return;

            var (vFrom, vTo, hFrom, hTo) = Global.GetTargetUnitsRange(position, Constants.VirtualWidth, Constants.VirtualHeight, Constants.UnitHeight, Constants.UnitWidth, Constants.WorldVCount, Constants.WorldHCount);

            for (int i = vFrom; i < vTo; i++)
            {
                for (int j = hFrom; j < hTo; j++)
                {
                    var unit = Units[i, j];
                    if (unit != null)
                    {
                        Color color = Color.White;
                        spriteBatch.Draw(_texture2DBGUnits, new Vector2(j * Constants.UnitWidth, i * Constants.UnitHeight), new Rectangle((int)unit.BG * Constants.UnitWidth, 0, Constants.UnitWidth, Constants.UnitHeight), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    }
                }
            }

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

                        spriteBatch.Draw(_texture2DFGUnits, new Vector2(j * Constants.UnitWidth, i * Constants.UnitHeight), new Rectangle((int)unit.FG * Constants.UnitWidth, 0, Constants.UnitWidth, Constants.UnitHeight), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
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
                            Color.White * _brightness);
                    }
                    else
                    {
                        spriteBatch.Draw(
                            _texture2DWhite,
                            new Rectangle(j * Constants.UnitWidth, i * Constants.UnitHeight, Constants.UnitWidth, Constants.UnitHeight),
                            Color.White * MathHelper.Min(_brightness, unit.FG != UnitFG.NONE ? unit.FGIntensity : unit.BG != UnitBG.NONE ? unit.BGIntensity : 1));
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