using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameApplication
{
    public class Application : Game
    {
        public Application()
        {
            Constants.Initialize();

            Window.AllowUserResizing = true;

            _ = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            Global.Game = this;
            Global.Window = Window;
            Global.GraphicsDevice = GraphicsDevice;
            Global.Content = Content;
            Global.ViewportAdapter = new ViewportAdapter(Window, GraphicsDevice, Constants.VirtualWidth, Constants.VirtualHeight);
            Global.Camera = new Camera(Global.ViewportAdapter)
            {
                Zoom = 1
            };
            Global.ScreenManager = new ScreenManager(this, new Dictionary<string, Screen> {
                { nameof(LogoScreen), new LogoScreen(this) },
                { nameof(MenuScreen), new MenuScreen(this) },
                { nameof(GameScreen), new GameScreen(this) },
                { nameof(OptionsScreen), new OptionsScreen(this) },
                { nameof(InformationScreen), new InformationScreen(this) },
            });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void BeginRun()
        {
            base.BeginRun();
        }

        protected override void Update(GameTime gameTime)
        {
            if (gameTime.GetElapsedSeconds() == 0) return;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // position = speed * gameTime.GetElapsedSeconds();

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
                Global.ScreenManager.GoTo(nameof(LogoScreen));
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
                Global.ScreenManager.GoTo(nameof(MenuScreen));
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
                Global.ScreenManager.GoTo(nameof(GameScreen));
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
                Global.ScreenManager.GoTo(nameof(OptionsScreen));
            if (Keyboard.GetState().IsKeyDown(Keys.D5))
                Global.ScreenManager.GoTo(nameof(InformationScreen));

            Global.ScreenManager.GoTo(nameof(GameScreen));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (gameTime.GetElapsedSeconds() == 0) return;

            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
