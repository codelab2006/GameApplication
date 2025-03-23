using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class PlayerPart : Sprite
    {
        public PlayerPart() { }

        public PlayerPart(Texture2D texture2D, Vector2 position) : base(texture2D)
        {
            Position = position;
        }

        public virtual void Update(float elapsedSeconds, Vector2 velocity, (bool tCollision, bool bCollision, bool lCollision, bool rCollision) collisions) { }
    }

    public abstract class PlayerHand : PlayerPart
    {
        protected readonly float _maxRotation = MathHelper.ToRadians(20);

        public PlayerHand() : base() { }

        public PlayerHand(Texture2D texture2D, Vector2 position, PlayerBody playerBody) : base(texture2D, position)
        {
            Origin = new(4, 0);
            position.X = GetPositionX(playerBody);
            position.Y = playerBody.Rectangle.Top + 1;
            Position = position;
        }

        protected abstract float GetPositionX(PlayerBody playerBody);
    }

    public abstract class PlayerLeg : PlayerPart
    {
        protected readonly float _maxRotation = MathHelper.ToRadians(20);
        public PlayerLeg() : base() { }

        public PlayerLeg(Texture2D texture2D, Vector2 position, PlayerBody playerBody) : base(texture2D, position)
        {
            Origin = new(Rectangle.Width / 2, 7);
            var bodyRectangle = playerBody.Rectangle;
            position.X = GetPositionX(playerBody);
            position.Y = bodyRectangle.Center.Y + 9;
            Position = position;
        }

        protected abstract float GetPositionX(PlayerBody playerBody);
    }
}