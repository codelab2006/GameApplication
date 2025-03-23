using Microsoft.Xna.Framework;

namespace GameApplication
{
    public struct CollisionRectangle(float x, float y, float width, float height)
    {
        public readonly float Left => x;

        public readonly float Right => Left + width;

        public readonly float Top => y;

        public readonly float Bottom => Top + height;

        public readonly Vector2 Center => new(x + (width / 2), y + (height / 2));

        public readonly bool Intersects(CollisionRectangle value)
        {
            return value.Left <= Right &&
                   Left <= value.Right &&
                   value.Top <= Bottom &&
                   Top <= value.Bottom;
        }
    }
}
