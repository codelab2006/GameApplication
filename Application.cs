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
            var viewportAdapter = new ViewportAdapter(Window, GraphicsDevice, Constants.VirtualWidth, Constants.VirtualHeight);
            var camera = new Camera(viewportAdapter);
            var screenManager = new ScreenManager(this, new Dictionary<string, Screen> {
                { nameof(LogoScreen), new LogoScreen(this) },
                { nameof(MenuScreen), new MenuScreen(this) },
                { nameof(GameScreen), new GameScreen(this) },
                { nameof(OptionsScreen), new OptionsScreen(this) },
                { nameof(InformationScreen), new InformationScreen(this) },
            });

            Global.Initialize(this, viewportAdapter, camera, screenManager);

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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
