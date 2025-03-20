using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class PlayerRightHand : PlayerPart
    {
        private int _direction = 1;
        private float _rotation = 0;
        private readonly float _maxRotation = MathHelper.ToRadians(20);

        public PlayerRightHand() { }

        public PlayerRightHand(Texture2D texture2D, Vector2 position, PlayerBody playerBody) : base(texture2D, position)
        {
            Origin = new(4, 0);
            var bodyRectangle = playerBody.Rectangle;
            position.X = bodyRectangle.Center.X - bodyRectangle.Width / 2 + 1;
            position.Y = playerBody.Rectangle.Top + 1;
            Position = position;
        }

        public override void Update(float elapsedSeconds, Vector2 velocity, (bool tCollision, bool bCollision, bool lCollision, bool rCollision) collisions)
        {
            if (!collisions.bCollision)
            {
                _direction = 1;
                _rotation = 0;
                Rotation = MathHelper.ToRadians(-160);
            }
            else
            {
                if (velocity.X == 0)
                {
                    _direction = 1;
                    _rotation = 0;
                    Rotation = _rotation;
                }
                else
                {
                    if (_rotation >= _maxRotation)
                        _direction = -1;
                    if (_rotation <= -_maxRotation)
                        _direction = 1;
                    _rotation += MathHelper.ToRadians(_direction * 180 * elapsedSeconds);
                    Rotation = _rotation;
                }
            }

            base.Update(elapsedSeconds, velocity, collisions);
        }
    }

}