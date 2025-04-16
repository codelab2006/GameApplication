using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class CircleLight
    {
        public static Texture2D NewInstance(int radius, Color lightColor, float falloff = 2.0f, float maxBrightness = 0.92f)
        {
            int size = radius * 2;
            var texture = Global.GameGraphicsDevice.CreateTexture2D(size, size);
            Color[] colorData = new Color[size * size];
            Vector2 center = new(radius, radius);
            float maxDistanceSquared = radius * radius;
            maxBrightness = MathHelper.Clamp(maxBrightness, 0f, 1f);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Vector2 pos = new(x, y);
                    float distSq = Vector2.DistanceSquared(pos, center);
                    float normalized = MathHelper.Clamp(1f - distSq / maxDistanceSquared, 0f, 1f);
                    float alpha = MathF.Pow(normalized, falloff) * maxBrightness;
                    byte a = (byte)(alpha * lightColor.A);
                    byte r = (byte)(alpha * lightColor.R);
                    byte g = (byte)(alpha * lightColor.G);
                    byte b = (byte)(alpha * lightColor.B);
                    colorData[y * size + x] = new Color(r, g, b, a);
                }
            }
            texture.SetData(colorData);
            return texture;
        }
    }

    public class RectangleLight
    {
        public static Texture2D NewInstance(int width, int height, Color lightColor, float falloff = 2.0f, float maxBrightness = 0.92f)
        {
            var texture = Global.GameGraphicsDevice.CreateTexture2D(width, height);
            Color[] colorData = new Color[width * height];
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;
            float invHalfWidth = 1f / halfWidth;
            float invHalfHeight = 1f / halfHeight;
            maxBrightness = MathHelper.Clamp(maxBrightness, 0f, 1f);
            for (int y = 0; y < height; y++)
            {
                float dy = Math.Abs(y - halfHeight) * invHalfHeight;
                for (int x = 0; x < width; x++)
                {
                    float dx = Math.Abs(x - halfWidth) * invHalfWidth;
                    float normalized = 1f - Math.Max(dx, dy);
                    if (normalized <= 0f)
                    {
                        colorData[y * width + x] = Color.Transparent;
                        continue;
                    }
                    float alphaFactor = MathF.Pow(normalized, falloff) * maxBrightness;
                    colorData[y * width + x] = new Color(
                        (byte)(alphaFactor * lightColor.R),
                        (byte)(alphaFactor * lightColor.G),
                        (byte)(alphaFactor * lightColor.B),
                        (byte)(alphaFactor * lightColor.A)
                    );
                }
            }
            texture.SetData(colorData);
            return texture;
        }
    }

    public class DiamondLight
    {
        public static Texture2D NewInstance(int width, int height, Color lightColor, float falloff = 2.0f, float maxBrightness = 0.92f)
        {
            var texture = Global.GameGraphicsDevice.CreateTexture2D(width, height);
            Color[] colorData = new Color[width * height];
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;
            float invHalfWidth = 1f / halfWidth;
            float invHalfHeight = 1f / halfHeight;
            maxBrightness = MathHelper.Clamp(maxBrightness, 0f, 1f);
            for (int y = 0; y < height; y++)
            {
                float dy = Math.Abs(y - halfHeight) * invHalfHeight;
                for (int x = 0; x < width; x++)
                {
                    float dx = Math.Abs(x - halfWidth) * invHalfWidth;
                    float normalized = 1f - (dx + dy);
                    if (normalized <= 0f)
                    {
                        colorData[y * width + x] = Color.Transparent;
                        continue;
                    }
                    float alphaFactor = MathF.Pow(normalized, falloff) * maxBrightness;
                    colorData[y * width + x] = new Color(
                        (byte)(alphaFactor * lightColor.R),
                        (byte)(alphaFactor * lightColor.G),
                        (byte)(alphaFactor * lightColor.B),
                        (byte)(alphaFactor * lightColor.A)
                    );
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