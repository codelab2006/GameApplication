using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class PlayerLeftHand : PlayerHand
    {
        private int _direction = -1;
        private float _rotation = 0;
        public PlayerLeftHand() { }

        public PlayerLeftHand(Texture2D texture2D, Vector2 position, PlayerBody playerBody) : base(texture2D, position, playerBody) { }

        protected override float GetPositionX(PlayerBody playerBody)
        {
            var bodyRectangle = playerBody.Rectangle;
            return bodyRectangle.Center.X + bodyRectangle.Width / 2 - 1;
        }

        public override void Update(float elapsedSeconds, Vector2 velocity, (bool tCollision, bool bCollision, bool lCollision, bool rCollision) collisions)
        {
            if (!collisions.bCollision)
            {
                _direction = -1;
                _rotation = 0;
                Rotation = MathHelper.ToRadians(-160);
            }
            else
            {
                if (velocity.X == 0)
                {
                    _direction = -1;
                    _rotation = 0;
                    Rotation = _rotation;
                }
                else
                {
                    if (_rotation <= -_maxRotation)
                        _direction = 1;
                    if (_rotation >= _maxRotation)
                        _direction = -1;
                    _rotation += MathHelper.ToRadians(_direction * 180 * elapsedSeconds);
                    Rotation = _rotation;
                }
            }

            base.Update(elapsedSeconds, velocity, collisions);
        }
    }

}