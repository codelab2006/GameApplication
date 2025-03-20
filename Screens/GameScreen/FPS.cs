using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class FPS
    {
        private SpriteFont? _font;
        private int _frameRate;
        private int _frameCounter;
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        private readonly SpriteBatch _spriteBatch = Global.GameGraphicsDevice.CreateSpriteBatch();

        public void LoadContent()
        {
            _font = Global.Content.Load<SpriteFont>("font");
        }

        public void Update(GameTime gameTime)
        {
            _frameCounter++;
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime > TimeSpan.FromSeconds(1))
            {
                _frameRate = _frameCounter;
                _frameCounter = 0;
                _elapsedTime -= TimeSpan.FromSeconds(1);
            }
        }

        public void Draw()
        {
            _spriteBatch.Begin(transformMatrix: Global.ViewportAdapter.GetScaleMatrix());
            _spriteBatch.DrawString(_font, $"FPS: {_frameRate}", new Vector2(0, 0), Color.Blue, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            _spriteBatch.End();
        }
    }
}
