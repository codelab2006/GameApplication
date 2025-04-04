using Microsoft.Xna.Framework;

namespace GameApplication
{
    public class GameScreen(Game game) : Screen(game)
    {
        private readonly Background _background = new();
        private readonly World _world = new();
        private Vector2 _position = Vector2.Zero;
        private Player? _player;
        private readonly Fog _fog = new();

        private readonly FPS _fps = new();

        public override void Initialize()
        {
            _world.Initialize();

            Global.World = _world;

            _position = new Vector2(_world.Width / 2, _world.Height / 3);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _fps.LoadContent();

            _world.LoadContent();

            _player = new()
            {
                Position = _position,
            };
            _player.LoadContent();

            Global.Camera.LookAt(_position);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_player != null)
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
            if (_player != null)
            {
                _background.DrawTarget(_spriteBatch);
                _player.DrawTarget(_spriteBatch);
                _fog.DrawTarget(_spriteBatch, _position);

                Global.GameGraphicsDevice.Clear(Color.Black);
                _background.Draw(_spriteBatch);
                _spriteBatch.Begin(transformMatrix: Global.Camera.GetViewMatrix());
                var rectangleF = _player.RectangleF;
                _world.Draw(_spriteBatch, _position, Global.GetTargetPeripheralUnitsRange(_player.RectangleF.Center, rectangleF.Width, rectangleF.Height, Constants.CollisionMargin, Constants.UnitHeight, Constants.UnitWidth, Constants.WorldVCount, Constants.WorldHCount));
                // _world.Draw(_spriteBatch, _position);
                _player?.Draw(_spriteBatch);
                _spriteBatch.End();
                _fog.Draw(_spriteBatch);
            }
            _fps.Draw();

            base.Draw(gameTime);
        }
    }
}