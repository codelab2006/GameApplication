using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class GameGraphicsDevice(GraphicsDevice graphicsDevice)
    {
        private readonly GraphicsDevice _instance = graphicsDevice;

        public void SetRenderTarget(RenderTarget2D? renderTarget)
        {
            _instance.SetRenderTarget(renderTarget);
            if (renderTarget == null)
                Global.ViewportAdapter.RefreshViewport();
        }

        public void Clear(Color color)
        {
            _instance.Clear(color);
        }

        public SpriteBatch CreateSpriteBatch()
        {
            return new SpriteBatch(_instance);
        }

        public RenderTarget2D CreateRenderTarget2D(int width, int height)
        {
            return new(_instance, width, height);
        }
    }
}