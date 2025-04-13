using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class AnimatedSprite : Sprite
    {
        private readonly int _frameWidth;
        private readonly int _frameCount;
        private readonly float _frameTime;
        private bool _isPlaying = true;
        private float _timer = 0f;
        private int _currentFrame = 0;
        public bool IsLooping { get; set; } = true;

        public AnimatedSprite(Texture2D? texture2D, int frameWidth, int frameHeight, int frameCount, float frameTime) : base(texture2D, new(0, 0, frameWidth, frameHeight))
        {
            _frameWidth = frameWidth;
            _frameCount = frameCount;
            _frameTime = frameTime;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isPlaying) return;

            _timer += gameTime.GetElapsedSeconds();
            if (_timer < _frameTime) return;
            _timer -= _frameTime;
            if (++_currentFrame < _frameCount) return;
            if (IsLooping)
            {
                _currentFrame = 0;
            }
            else
            {
                _currentFrame = _frameCount - 1;
                _isPlaying = false;
            }
        }

        public void Play()
        {
            _isPlaying = true;
        }

        public void Pause()
        {
            _isPlaying = false;
        }

        public void Stop()
        {
            _isPlaying = false;
            _currentFrame = 0;
            _timer = 0f;
        }

        public void SetFrame(int frameIndex)
        {
            if (frameIndex >= 0 && frameIndex < _frameCount)
            {
                _currentFrame = frameIndex;
                _timer = 0f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _sourceRectangle = new(_currentFrame * _frameWidth, 0, _sourceRectangle.Width, _sourceRectangle.Height);
            base.Draw(spriteBatch);
        }
    }
}