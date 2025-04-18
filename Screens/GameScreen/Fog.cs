using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class Fog
    {
        private readonly RenderTarget2D _renderTarget2D;
        private readonly BlendState _multiply = new()
        {
            ColorSourceBlend = Blend.Zero,
            ColorDestinationBlend = Blend.SourceColor,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.Zero,
            AlphaDestinationBlend = Blend.SourceAlpha,
            AlphaBlendFunction = BlendFunction.Add
        };

        private readonly Dictionary<string, ILightRenderer> _lightRenderers = [];

        public Fog()
        {
            _renderTarget2D = Global.GameGraphicsDevice.CreateHDRRenderTarget2D(Constants.VirtualWidth, Constants.VirtualHeight);
        }

        public void AddLightRenderer(string name, ILightRenderer lightRenderer)
        {
            _lightRenderers.Add(name, lightRenderer);
        }

        public void RemoveLightRenderer(string name)
        {
            _lightRenderers.Remove(name);
        }

        public void DrawTarget(SpriteBatch spriteBatch, Vector2 position)
        {
            var graphicsDevice = Global.GameGraphicsDevice;
            graphicsDevice.SetRenderTarget(_renderTarget2D);
            graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(blendState: BlendState.Additive, transformMatrix: Global.Camera.GetViewMatrix());
            foreach (var renderer in _lightRenderers.Values)
            {
                renderer.DrawLight(spriteBatch, position);
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