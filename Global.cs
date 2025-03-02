using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#pragma warning disable CS8618

namespace GameApplication
{
    public static class Global
    {
        public static Game Game { private set; get; }
        public static GameWindow Window { private set; get; }
        public static GraphicsDevice GraphicsDevice { private set; get; }
        public static ContentManager Content { private set; get; }
        public static ViewportAdapter ViewportAdapter { private set; get; }
        public static Camera Camera { private set; get; }
        public static ScreenManager ScreenManager { private set; get; }

        public static void Initialize(Game game, ViewportAdapter viewportAdapter, Camera camera, ScreenManager screenManager)
        {
            Game = game;
            Window = game.Window;
            GraphicsDevice = game.GraphicsDevice;
            Content = game.Content;
            ViewportAdapter = viewportAdapter;
            Camera = camera;
            ScreenManager = screenManager;
        }

        public static (int vFrom, int vTo, int hFrom, int hTo) GetDrawableRange(Vector2 position)
        {
            int vFromPixel = (int)(position.Y - Constants.VirtualHeight / 2);
            int vFrom = vFromPixel / Constants.UnitHeight;
            if (vFrom < 0) vFrom = 0;
            if (vFrom > Constants.WorldVCount) vFrom = Constants.WorldVCount;

            int vToPixel = (int)(position.Y + Constants.VirtualHeight / 2);
            int vTo = vToPixel / Constants.UnitHeight;
            if (vTo * Constants.UnitHeight < vToPixel) vTo += 1;
            if (vTo > Constants.WorldVCount) vTo = Constants.WorldVCount;
            if (vTo < 0) vTo = 0;

            int hFromPixel = (int)(position.X - Constants.VirtualWidth / 2);
            int hFrom = hFromPixel / Constants.UnitWidth;
            if (hFrom < 0) hFrom = 0;
            if (hFrom > Constants.WorldHCount) hFrom = Constants.WorldHCount;

            int hToPixel = (int)(position.X + Constants.VirtualWidth / 2);
            int hTo = hToPixel / Constants.UnitWidth;
            if (hTo * Constants.UnitWidth < hToPixel) hTo += 1;
            if (hTo > Constants.WorldHCount) hTo = Constants.WorldHCount;
            if (hTo < 0) hTo = 0;

            System.Console.WriteLine((vFrom, vTo, hFrom, hTo));
            return (vFrom, vTo, hFrom, hTo);
        }
    }
}