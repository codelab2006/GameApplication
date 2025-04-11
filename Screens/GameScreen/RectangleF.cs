using Microsoft.Xna.Framework;

namespace GameApplication
{
    public struct RectangleF(float x, float y, float width, float height)
    {
        public readonly float X => x;

        public readonly float Y => y;

        public readonly float Width => width;

        public readonly float Height => height;

        public readonly float Left => x;

        public readonly float Top => y;

        public readonly float Right => x + width;

        public readonly float Bottom => y + height;

        public readonly Vector2 Center => new(x + (width / 2), y + (height / 2));

        public readonly bool Intersects(RectangleF value)
        {
            return value.Left < Right &&
                   Left < value.Right &&
                   value.Top < Bottom &&
                   Top < value.Bottom;
        }

        public readonly bool Contains(Vector2 point)
        {
            return point.X >= Left && point.X < Right &&
                   point.Y >= Top && point.Y < Bottom;
        }
    }
}
