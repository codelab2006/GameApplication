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

        public override void Initialize()
        {
            _world.Initialize();

            _position = new Vector2(_world.Width / 2, _world.Height / 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _world.LoadContent();

            _player = new(Global.Content.Load<Texture2D>("o"))
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
                if (state.IsKeyDown(Keys.Down)) direction += Vector2.UnitY;
                if (state.IsKeyDown(Keys.Up)) direction -= Vector2.UnitY;
                if (state.IsKeyDown(Keys.Left)) direction -= Vector2.UnitX;
                if (state.IsKeyDown(Keys.Right)) direction += Vector2.UnitX;
                Move(direction * 100 * gameTime.GetElapsedSeconds());
                _player.Position = _position;
                Global.Camera.LookAt(_player.Position);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: Global.Camera.GetViewMatrix());

            _world.Draw(_spriteBatch, _position);

            _player?.Draw(_spriteBatch);
            // _player?.Draw(_spriteBatch, new Vector2(_player.Position.X - Constants.VirtualWidth / 2, _player.Position.Y));
            // _player?.Draw(_spriteBatch, new Vector2(_player.Position.X + Constants.VirtualWidth / 2, _player.Position.Y));
            // _player?.Draw(_spriteBatch, new Vector2(_player.Position.X, _player.Position.Y - Constants.VirtualHeight / 2));
            // _player?.Draw(_spriteBatch, new Vector2(_player.Position.X, _player.Position.Y + Constants.VirtualHeight / 2));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}