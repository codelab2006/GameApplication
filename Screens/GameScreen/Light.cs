using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class CircleLight
    {
        public static Texture2D NewInstance(int radius, Color lightColor)
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
                    alpha *= 0.92f;

                    colorData[y * size + x] = lightColor * alpha;
                }
            }
            texture.SetData(colorData);
            return texture;
        }
    }

    public class RectangleLight
    {
        public static Texture2D NewInstance(int width, int height, Color lightColor, bool horizontalFade = true, bool verticalFade = true)
        {
            var texture = Global.GameGraphicsDevice.CreateTexture2D(width, height);
            Color[] colorData = new Color[width * height];
            Vector2 center = new(width / 2f, height / 2f);
            float maxX = width / 2f;
            float maxY = height / 2f;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float dx = horizontalFade ? MathF.Abs(x - center.X) / maxX : 0f;
                    float dy = verticalFade ? MathF.Abs(y - center.Y) / maxY : 0f;
                    float distance = MathF.Max(dx, dy);
                    float alpha = MathHelper.Clamp(1f - distance, 0f, 1f);
                    alpha *= alpha;
                    alpha *= 0.92f;
                    colorData[y * width + x] = lightColor * alpha;
                }
            }
            texture.SetData(colorData);
            return texture;
        }
    }

    public class DiamondLight
    {
        public static Texture2D NewInstance(int width, int height, Color lightColor)
        {
            var texture = Global.GameGraphicsDevice.CreateTexture2D(width, height);
            Color[] colorData = new Color[width * height];
            Vector2 center = new(width / 2f, height / 2f);
            float maxX = width / 2f;
            float maxY = height / 2f;
            float maxDistance = maxX + maxY;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float dx = MathF.Abs(x - center.X);
                    float dy = MathF.Abs(y - center.Y);
                    float distance = (dx + dy) / maxDistance;
                    float alpha = MathHelper.Clamp(1f - distance, 0f, 1f);
                    alpha *= alpha;
                    alpha *= 0.92f;
                    colorData[y * width + x] = lightColor * alpha;
                }
            }
            texture.SetData(colorData);
            return texture;
        }
    }

    public interface ILightRenderer
    {
        void DrawLight(SpriteBatch spriteBatch, Vector2 position);
    }
}