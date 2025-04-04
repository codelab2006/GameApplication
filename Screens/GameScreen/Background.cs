using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class Background
    {
        private readonly RenderTarget2D _renderTarget2D;
        private readonly Texture2D _texture2D = Global.GameGraphicsDevice.CreateTexture2D(1, 1);

        public Background()
        {
            _renderTarget2D = Global.GameGraphicsDevice.CreateRenderTarget2D(Constants.VirtualWidth, Constants.VirtualHeight);
            _texture2D.SetData([new Color(0, 0, 255, 255)]);
        }

        public void DrawTarget(SpriteBatch spriteBatch)
        {
            var graphicsDevice = Global.GameGraphicsDevice;
            graphicsDevice.SetRenderTarget(_renderTarget2D);
            graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(_texture2D, new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight), Color.White);
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Global.ViewportAdapter.GetScaleMatrix());
            spriteBatch.Draw(_renderTarget2D, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}