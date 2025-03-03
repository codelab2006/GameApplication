using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameApplication
{
    public class GameScreen(Game game) : Screen(game)
    {
        private readonly World _world = new();
        private Vector2 _position;
        private Player? _player;

        private readonly FPS _fps = new();

        public override void Initialize()
        {
            _world.Initialize();

            _position = new Vector2(_world.Width / 2, _world.Height / 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _fps.LoadContent();

            _world.LoadContent();

            _player = new(Global.Content.Load<Texture2D>("player"))
            {
                Position = _position
            };

            Global.Camera.LookAt(_player.Position);

            base.LoadContent();
        }

        private void Move(Vector2 direction)
        {
            _position += Vector2.Transform(direction, Matrix.CreateRotationZ(0));
        }

        public override void Update(GameTime gameTime)
        {
            if (_player is not null)
            {
                var direction = Vector2.Zero;
                var state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.W)) direction -= Vector2.UnitY;
                if (state.IsKeyDown(Keys.S)) direction += Vector2.UnitY;
                if (state.IsKeyDown(Keys.A)) direction -= Vector2.UnitX;
                if (state.IsKeyDown(Keys.D)) direction += Vector2.UnitX;
                Move(direction * 240 * gameTime.GetElapsedSeconds());
                _player.Position = _position;
                Global.Camera.LookAt(_player.Position);
            }

            _fps.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: Global.Camera.GetViewMatrix());

            _world.Draw(_spriteBatch, _position);

            _player?.Draw(_spriteBatch);

            _spriteBatch.End();

            _fps.Draw();

            base.Draw(gameTime);
        }
    }
}