using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class Background
    {
        private readonly RenderTarget2D _renderTarget2D;
        private readonly Texture2D _texture2D = Global.GameGraphicsDevice.CreateTexture2D(1, 1);

        private readonly int _aDayTime;
        private float _progress = 0.25f;

        public Background(int aDayTime)
        {
            _aDayTime = aDayTime;
            _renderTarget2D = Global.GameGraphicsDevice.CreateRenderTarget2D(Constants.VirtualWidth, Constants.VirtualHeight);
            _texture2D.SetData([Color.Black]);
        }

        public void Update(GameTime gameTime)
        {
            _progress += gameTime.GetElapsedSeconds() / _aDayTime;
            if (_progress > 1f) _progress -= 1f;
            _texture2D.SetData([DayNightCycle.GetBackgroundColor(_progress)]);
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