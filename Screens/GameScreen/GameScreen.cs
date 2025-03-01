using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class GameScreen(Game game) : Screen(game)
    {
        private World? _world;
        private WorldRenderer? _worldRenderer;
        private Vector2 _position;
        private Player? _player;

        public override void Initialize()
        {
            _world = new World();
            _worldRenderer = new WorldRenderer();
            _position = new Vector2(_world.Width / 2, _world.Height / 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _player = new(Global.Content.Load<Texture2D>("o"))
            {
                Position = _position
            };

            Global.Camera.LookAt(_position);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: Global.Camera.GetViewMatrix());
            _player?.Draw(_spriteBatch);
            _player?.Draw(_spriteBatch, new Vector2(_player.Position.X - Constants.VirtualWidth / 2, _player.Position.Y));
            _player?.Draw(_spriteBatch, new Vector2(_player.Position.X + Constants.VirtualWidth / 2, _player.Position.Y));
            _player?.Draw(_spriteBatch, new Vector2(_player.Position.X, _player.Position.Y - Constants.VirtualHeight / 2));
            _player?.Draw(_spriteBatch, new Vector2(_player.Position.X, _player.Position.Y + Constants.VirtualHeight / 2));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}