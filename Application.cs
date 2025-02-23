using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameApplication
{
    public class Application : Game
    {
        public ViewportAdapter ViewportAdapter { private set; get; }
        public Camera Camera { private set; get; }
        public SpriteBatch SpriteBatch { private set; get; }

        private Texture2D _texture2DO;
        private Texture2D _texture2DXP;

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
            ViewportAdapter = new ViewportAdapter(Window, GraphicsDevice, Constants.VirtualWidth, Constants.VirtualHeight);
            Camera = new Camera(ViewportAdapter);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            _texture2DO = Content.Load<Texture2D>("o");
            _texture2DXP = Content.Load<Texture2D>("xp");
        }

        protected override void BeginRun()
        {
            Camera.LookAt(_texture2DXP.Bounds.Center.ToVector2());

            base.BeginRun();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // position = speed * gameTime.GetElapsedSeconds();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            SpriteBatch.Draw(_texture2DXP, Vector2.Zero, Color.White);
            SpriteBatch.Draw(_texture2DO, _texture2DXP.Bounds.Center.ToVector2(), _texture2DO.Bounds, Color.White, 0, new Vector2(_texture2DO.Width / 2, _texture2DO.Height / 2), 1, SpriteEffects.None, 0);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
