using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameApplication
{
    public class GameScreen(Game game) : Screen(game)
    {
        private readonly World _world = new();
        private Vector2 _position = Vector2.Zero;
        private Player? _player;

        private readonly FPS _fps = new();

        private bool _debugKeyDown = false;

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
                Position = _position,
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
                if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.D))
                {
                    if (state.IsKeyDown(Keys.W)) direction -= Vector2.UnitY;
                    if (state.IsKeyDown(Keys.S)) direction += Vector2.UnitY;
                    if (state.IsKeyDown(Keys.A)) direction -= Vector2.UnitX;
                    if (state.IsKeyDown(Keys.D)) direction += Vector2.UnitX;
                    Move(direction * (int)(240 * gameTime.GetElapsedSeconds()));
                }
                if (!_debugKeyDown && (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Right)))
                {
                    if (state.IsKeyDown(Keys.Up)) direction -= Vector2.UnitY;
                    if (state.IsKeyDown(Keys.Down)) direction += Vector2.UnitY;
                    if (state.IsKeyDown(Keys.Left)) direction -= Vector2.UnitX;
                    if (state.IsKeyDown(Keys.Right)) direction += Vector2.UnitX;
                    Move(direction);
                    _debugKeyDown = true;
                }
                if (state.IsKeyUp(Keys.Up) && state.IsKeyUp(Keys.Down) && state.IsKeyUp(Keys.Left) && state.IsKeyUp(Keys.Right))
                    _debugKeyDown = false;


                _player.Position = _position;
                Global.Camera.LookAt(_player.Position);
            }

            _fps.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: Global.Camera.GetViewMatrix());

            if (_player is not null)
            {
                var rectangle = _player.Rectangle;
                _world.Draw(_spriteBatch, _position, Global.GetTargetPeripheralUnitsRange(_player.Position, rectangle.Width, rectangle.Height, Constants.CollisionMargin, Constants.UnitHeight, Constants.UnitWidth, Constants.WorldVCount, Constants.WorldHCount));

                // _world.Draw(_spriteBatch, _position);

                _player?.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            _fps.Draw();

            base.Draw(gameTime);
        }
    }
}