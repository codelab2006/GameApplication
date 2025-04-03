using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class Fog
    {
        private readonly RenderTarget2D _renderTarget2D;
        private readonly Texture2D _texture2DWhiteA100 = Global.GameGraphicsDevice.CreateTexture2D(1, 1);
        private readonly Texture2D _textureWhite2DA50 = Global.GameGraphicsDevice.CreateTexture2D(1, 1);
        private readonly BlendState _multiply = new()
        {
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.Zero
        };

        public Fog()
        {
            _renderTarget2D = Global.GameGraphicsDevice.CreateRenderTarget2D(Constants.VirtualWidth, Constants.VirtualHeight);
            _texture2DWhiteA100.SetData([new Color(255, 255, 255, 128)]);
            _textureWhite2DA50.SetData([new Color(255, 255, 255, 128)]);
        }

        public void DrawTarget(SpriteBatch spriteBatch, Vector2 position)
        {
            var graphicsDevice = Global.GameGraphicsDevice;
            graphicsDevice.SetRenderTarget(_renderTarget2D);
            graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(blendState: BlendState.Additive, transformMatrix: Global.Camera.GetViewMatrix());
            var (vFrom, vTo, hFrom, hTo) = Global.GetTargetUnitsRange(position, Constants.VirtualWidth, Constants.VirtualHeight, Constants.UnitHeight, Constants.UnitWidth, Constants.WorldVCount, Constants.WorldHCount);
            for (int i = vFrom; i < vTo; i++)
            {
                for (int j = hFrom; j < hTo; j++)
                {
                    var unit = Global.World.Units[i, j];
                    if (unit == null)
                    {
                        spriteBatch.Draw(_texture2DWhiteA100, new Rectangle(j * Constants.UnitWidth, i * Constants.UnitHeight, Constants.UnitWidth, Constants.UnitHeight), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(_textureWhite2DA50, new Rectangle(j * Constants.UnitWidth, i * Constants.UnitHeight, Constants.UnitWidth, Constants.UnitHeight), Color.White);
                    }
                }
            }
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: _multiply);
            spriteBatch.Draw(_renderTarget2D, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}