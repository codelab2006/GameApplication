using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class Light
    {
        public static Texture2D NewInstance(int radius)
        {
            int size = radius * 2;
            var texture = Global.GameGraphicsDevice.CreateTexture2D(size, size);
            Color[] colorData = new Color[size * size];
            Vector2 center = new(radius, radius);
            float maxDistance = radius;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Vector2 position = new(x, y);
                    float distance = Vector2.Distance(position, center);
                    float alpha = MathHelper.Clamp(1f - (distance / maxDistance), 0f, 1f);
                    alpha *= alpha;

                    colorData[y * size + x] = new Color(255, 255, 255, alpha);
                }
            }
            texture.SetData(colorData);
            return texture;
        }
    }
}