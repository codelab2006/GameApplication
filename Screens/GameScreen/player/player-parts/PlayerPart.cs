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
}