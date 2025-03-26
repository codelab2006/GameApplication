using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#pragma warning disable CS8618

namespace GameApplication
{
    public static class Global
    {
        public static Game Game { set; get; }
        public static GameWindow Window { set; get; }
        public static GameGraphicsDevice GameGraphicsDevice { set; get; }
        public static ContentManager Content { set; get; }
        public static ViewportAdapter ViewportAdapter { set; get; }
        public static Camera Camera { set; get; }
        public static ScreenManager ScreenManager { set; get; }
        public static World World { get; set; }

        public static (Vector2 position, bool tCollision, bool bCollision, bool lCollision, bool rCollision) MoveBasedOnSurroundings(
                Vector2 currentPosition,
                Vector2 velocity,
                int collisionStep,
                Func<Vector2, RectangleF> getRectangle,
                int margin, int unitHeight, int unitWidth, int totalVCount, int totalHCount,
                IUnit[,] units,
                float elapsedSeconds)
        {
            bool tCollision = false, bCollision = false, lCollision = false, rCollision = false;
            if (velocity == Vector2.Zero || elapsedSeconds == 0) return (currentPosition, tCollision, bCollision, lCollision, rCollision);

            var frameVelocity = velocity * elapsedSeconds;
            if (frameVelocity == Vector2.Zero || float.IsNaN(frameVelocity.X) || float.IsNaN(frameVelocity.Y)) return (currentPosition, tCollision, bCollision, lCollision, rCollision);

            var normalizedV = Vector2.Normalize(frameVelocity);
            if (normalizedV == Vector2.Zero || float.IsNaN(normalizedV.X) || float.IsNaN(normalizedV.Y)) return (currentPosition, tCollision, bCollision, lCollision, rCollision);

            var newPosition = Move(currentPosition, frameVelocity);
            Vector2 stepVelocity = normalizedV * collisionStep * elapsedSeconds;
            if (stepVelocity.LengthSquared() > frameVelocity.LengthSquared()) stepVelocity = frameVelocity;

            float? newPositionY = null, newPositionX = null;
            var rectangle = getRectangle(currentPosition);
            float rectWidth = rectangle.Width, rectHeight = rectangle.Height;
            var (vIndexes, hIndexes) = GetTargetVHPeripheralUnitsIndexes(rectangle.Center,
                rectWidth,
                rectHeight,
                margin,
                unitHeight,
                unitWidth,
                totalVCount,
                totalHCount
            );
            while (stepVelocity.LengthSquared() <= frameVelocity.LengthSquared())
            {
                var stepVCollisionRectangle = getRectangle(Move(currentPosition, new(0, stepVelocity.Y + (stepVelocity.Y < 0 ? -1 : 0))));
                if (!bCollision && !tCollision)
                {
                    foreach (var (vi, hi) in vIndexes)
                    {
                        IUnit? unit = units[vi, hi];
                        if (unit == null || !unit.IsStatic) continue;

                        var uRectangle = new RectangleF(hi * unitWidth, vi * unitHeight, unitWidth, unitHeight);
                        if (!uRectangle.Intersects(stepVCollisionRectangle)) continue;
                        if (stepVelocity.Y > 0 && uRectangle.Center.Y > stepVCollisionRectangle.Center.Y)
                        {
                            newPositionY = newPositionY.HasValue ? MathF.Min(newPositionY.Value, uRectangle.Top - (float)rectHeight / 2) : uRectangle.Top - (float)rectHeight / 2;
                            bCollision = true;
                        }

                        if (stepVelocity.Y < 0 && uRectangle.Center.Y < stepVCollisionRectangle.Center.Y)
                        {
                            newPositionY = newPositionY.HasValue ? MathF.Max(newPositionY.Value, uRectangle.Bottom + (float)rectHeight / 2) : uRectangle.Bottom + (float)rectHeight / 2;
                            tCollision = true;
                        }
                    }
                }
                var stepHCollisionRectangle = getRectangle(Move(currentPosition, new(stepVelocity.X + (stepVelocity.X < 0 ? -1 : 0), 0)));
                if (!rCollision && !lCollision)
                {
                    foreach (var (vi, hi) in hIndexes)
                    {
                        IUnit? unit = units[vi, hi];
                        if (unit == null || !unit.IsStatic) continue;

                        var uRectangle = new RectangleF(hi * unitWidth, vi * unitHeight, unitWidth, unitHeight);
                        if (!uRectangle.Intersects(stepHCollisionRectangle)) continue;
                        if (stepVelocity.X > 0 && uRectangle.Center.X > stepHCollisionRectangle.Center.X)
                        {
                            newPositionX = newPositionX.HasValue ? MathF.Min(newPositionX.Value, uRectangle.Left - (float)rectWidth / 2) : uRectangle.Left - (float)rectWidth / 2;
                            rCollision = true;
                        }
                        if (stepVelocity.X < 0 && uRectangle.Center.X < stepHCollisionRectangle.Center.X)
                        {
                            newPositionX = newPositionX.HasValue ? MathF.Max(newPositionX.Value, uRectangle.Right + (float)rectWidth / 2) : uRectangle.Right + (float)rectWidth / 2;
                            lCollision = true;
                        }
                    }
                }
                if ((newPositionY.HasValue && newPositionX.HasValue) || stepVelocity.LengthSquared() == frameVelocity.LengthSquared()) break;
                stepVelocity += normalizedV * collisionStep * elapsedSeconds;
                if (stepVelocity.LengthSquared() > frameVelocity.LengthSquared()) stepVelocity = frameVelocity;
            }

            if (newPositionY != null) newPosition.Y = MathF.Floor((float)newPositionY);
            if (newPositionX != null) newPosition.X = MathF.Floor((float)newPositionX);

            return (newPosition, tCollision, bCollision, lCollision, rCollision);
        }

        private static ((int v, int h)[] v, (int v, int h)[] h) GetTargetVHPeripheralUnitsIndexes(Vector2 center, float width, float height, int margin, int unitHeight, int unitWidth, int totalVCount, int totalHCount)
        {
            var (vTFrom, vTTo, vBFrom, vBTo, hLFrom, hLTo, hRFrom, hRTo) = GetTargetPeripheralUnitsRange(center, width, height, margin, unitHeight, unitWidth, totalVCount, totalHCount);

            var vResult = new List<(int i, int j)>((vBTo - vTFrom) * (hRFrom - hLTo));
            for (int i = vTFrom; i < vBTo; i++)
            {
                for (int j = hLFrom; j < hRTo; j++)
                {
                    if ((i >= vTFrom && i < vTTo) || (i >= vBFrom && i < vBTo))
                        vResult.Add((i, j));
                }
            }

            var hResult = new List<(int i, int j)>((vBFrom - vTTo) * (hRTo - hLFrom));
            for (int j = hLFrom; j < hRTo; j++)
            {
                for (int i = vTFrom; i < vBTo; i++)
                {
                    if ((j >= hLFrom && j < hLTo) || (j >= hRFrom && j < hRTo))
                        hResult.Add((i, j));
                }
            }

            return (v: [.. vResult], h: [.. hResult]);
        }

        public static (int vTFrom, int vTTo, int vBFrom, int vBTo, int hLFrom, int hLTo, int hRFrom, int hRTo) GetTargetPeripheralUnitsRange(Vector2 center, float width, float height, int margin, int unitHeight, int unitWidth, int totalVCount, int totalHCount)
        {
            var (vTFrom, vBTo, hLFrom, hRTo) = GetTargetUnitsRange(center, width + margin * 2, height + margin * 2, unitHeight, unitWidth, totalVCount, totalHCount);
            var (vTTo, vBFrom, hLTo, hRFrom) = GetTargetUnitsRange(center, width, height, unitHeight, unitWidth, totalVCount, totalHCount);
            return (vTFrom, vTTo, vBFrom, vBTo, hLFrom, hLTo, hRFrom, hRTo);
        }

        public static (int vFrom, int vTo, int hFrom, int hTo) GetTargetUnitsRange(Vector2 center, float width, float height, int unitHeight, int unitWidth, int totalVCount, int totalHCount)
        {
            var (vFrom, vTo) = GetTargetVUnitsRange(center, height, unitHeight, totalVCount);
            var (hFrom, hTo) = GetTargetHUnitsRange(center, width, unitWidth, totalHCount);
            return (vFrom, vTo, hFrom, hTo);
        }

        private static (int vFrom, int vTo) GetTargetVUnitsRange(Vector2 center, float height, float unitHeight, int totalVCount)
        {
            return GetTargetUnitsRange(center.Y, height, unitHeight, totalVCount);
        }

        private static (int hFrom, int hTo) GetTargetHUnitsRange(Vector2 center, float width, float unitWidth, int totalHCount)
        {
            return GetTargetUnitsRange(center.X, width, unitWidth, totalHCount);
        }

        private static (int from, int to) GetTargetUnitsRange(float center, float size, float unitSize, int totalCount)
        {
            int fromPixel = (int)(center - size / 2);
            int from = (int)(fromPixel / unitSize);
            if (from < 0) from = 0;
            if (from > totalCount) from = totalCount;

            int toPixel = (int)(center + size / 2);
            int to = (int)(toPixel / unitSize);
            if (to * unitSize < toPixel) to += 1;
            if (to > totalCount) to = totalCount;
            if (to < 0) to = 0;

            return (from, to);
        }

        public static Vector2 Move(Vector2 position, Vector2 velocity, float elapsedSeconds)
        {
            return position + velocity * elapsedSeconds;
        }

        public static Vector2 Move(Vector2 position, Vector2 velocity)
        {
            return position + velocity;
        }

        public static Vector2 UpdateVelocityByGravity(Vector2 velocity, Vector2 gravityAcceleration, int? maxVerticalVelocity)
        {
            velocity += gravityAcceleration;

            if (maxVerticalVelocity != null && MathF.Abs(velocity.Y) > maxVerticalVelocity)
                velocity.Y = (float)(MathF.Sign(velocity.Y) * maxVerticalVelocity);

            return velocity;
        }
    }
}