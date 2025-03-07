using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class GameScreen(Game game) : Screen(game)
    {
        private readonly World _world = new();
        private Vector2 _position = Vector2.Zero;
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
                Position = _position,
            };

            Global.Camera.LookAt(_position);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_player is not null)
            {
                _player.Update(gameTime);
                _position = _player.Position;
                Global.Camera.LookAt(_position);
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
                _world.Draw(_spriteBatch, _position, Global.GetTargetPeripheralUnitsRange(_position, rectangle.Width, rectangle.Height, Constants.CollisionMargin, Constants.UnitHeight, Constants.UnitWidth, Constants.WorldVCount, Constants.WorldHCount));

                // _world.Draw(_spriteBatch, _position);

                _player?.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            _fps.Draw();

            base.Draw(gameTime);
        }
    }
}