using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#pragma warning disable CS8618

namespace GameApplication
{
    public static class Global
    {
        public static Game Game { set; get; }
        public static GameWindow Window { set; get; }
        public static GraphicsDevice GraphicsDevice { set; get; }
        public static ContentManager Content { set; get; }
        public static ViewportAdapter ViewportAdapter { set; get; }
        public static Camera Camera { set; get; }
        public static ScreenManager ScreenManager { set; get; }

        public static (int vTFrom, int vTTo, int vBFrom, int vBTo, int hLFrom, int hLTo, int hRFrom, int hRTo) GetTargetPeripheralUnitsRange(Vector2 center, int width, int height, int margin, int unitHeight, int unitWidth, int totalVCount, int totalHCount)
        {
            var (vTFrom, vBTo, hLFrom, hRTo) = GetTargetUnitsRange(center, width + margin * 2, height + margin * 2, unitHeight, unitWidth, totalVCount, totalHCount);
            var (vTTo, vBFrom, hLTo, hRFrom) = GetTargetUnitsRange(center, width, height, unitHeight, unitWidth, totalVCount, totalHCount);
            return (vTFrom, vTTo, vBFrom, vBTo, hLFrom, hLTo, hRFrom, hRTo);
        }

        public static (int v, int h)[] GetTargetPeripheralUnitsIndexes(Vector2 center, int width, int height, int margin, int unitHeight, int unitWidth, int totalVCount, int totalHCount)
        {
            var (vTFrom, vTTo, vBFrom, vBTo, hLFrom, hLTo, hRFrom, hRTo) = GetTargetPeripheralUnitsRange(center, width, height, margin, unitHeight, unitWidth, totalVCount, totalHCount);
            int estimatedCount = (vBTo - vTFrom) * (hRTo - hLFrom);
            var result = new List<(int i, int j)>(estimatedCount);
            for (int i = vTFrom; i < vBTo; i++)
            {
                for (int j = hLFrom; j < hRTo; j++)
                {
                    if ((i >= vTFrom && i < vBTo && ((j >= hLFrom && j < hLTo) || (j >= hRFrom && j < hRTo))) || (j >= hLFrom && j < hRTo && ((i >= vTFrom && i < vTTo) || (i >= vBFrom && i < vBTo))))
                        result.Add((i, j));
                }
            }
            return [.. result];
        }

        public static (int vFrom, int vTo, int hFrom, int hTo) GetTargetUnitsRange(Vector2 center, int width, int height, int unitHeight, int unitWidth, int totalVCount, int totalHCount)
        {
            int vFromPixel = (int)center.Y - height / 2;
            int vFrom = vFromPixel / unitHeight;
            if (vFrom < 0) vFrom = 0;
            if (vFrom > totalVCount) vFrom = totalVCount;

            int vToPixel = (int)center.Y + height / 2;
            int vTo = vToPixel / unitHeight;
            if (vTo * unitHeight < vToPixel) vTo += 1;
            if (vTo > totalVCount) vTo = totalVCount;
            if (vTo < 0) vTo = 0;

            int hFromPixel = (int)center.X - width / 2;
            int hFrom = hFromPixel / unitWidth;
            if (hFrom < 0) hFrom = 0;
            if (hFrom > totalHCount) hFrom = totalHCount;

            int hToPixel = (int)center.X + width / 2;
            int hTo = hToPixel / unitWidth;
            if (hTo * unitWidth < hToPixel) hTo += 1;
            if (hTo > totalHCount) hTo = totalHCount;
            if (hTo < 0) hTo = 0;
            return (vFrom, vTo, hFrom, hTo);
        }
    }
}