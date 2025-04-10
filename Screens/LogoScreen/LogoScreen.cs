using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class LogoScreen(Game game) : Screen(game)
    {
        private Logo? _logo;

        protected override void LoadContent()
        {
            _logo = new(Global.Content.Load<Texture2D>("monogame"));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: Global.Camera.GetViewMatrix());
            _logo?.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}