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
    }
}