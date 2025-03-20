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

        public static (int vTFrom, int vTTo, int vBFrom, int vBTo, int hLFrom, int hLTo, int hRFrom, int hRTo) GetTargetPeripheralUnitsRange(Vector2 center, int width, int height, int margin, int unitHeight, int unitWidth, int totalVCount, int totalHCount)
        {
            var (vTFrom, vBTo, hLFrom, hRTo) = GetTargetUnitsRange(center, width + margin * 2, height + margin * 2, unitHeight, unitWidth, totalVCount, totalHCount);
            var (vTTo, vBFrom, hLTo, hRFrom) = GetTargetUnitsRange(center, width, height, unitHeight, unitWidth, totalVCount, totalHCount);
            return (vTFrom, vTTo, vBFrom, vBTo, hLFrom, hLTo, hRFrom, hRTo);
        }

        public static ((int v, int h)[] v, (int v, int h)[] h) GetTargetVHPeripheralUnitsIndexes(Vector2 center, int width, int height, int margin, int unitHeight, int unitWidth, int totalVCount, int totalHCount)
        {
            var (vTFrom, vBTo) = GetTargetVUnitsRange(center, height + margin * 2, unitHeight, totalVCount);
            var (hLFrom, hRTo) = GetTargetHUnitsRange(center, width + margin * 2, unitWidth, totalHCount);
            var (vTTo, vBFrom, hLTo, hRFrom) = GetTargetUnitsRange(center, width, height, unitHeight, unitWidth, totalVCount, totalHCount);

            var vResult = new List<(int i, int j)>((vBTo - vTFrom) * (hRFrom - hLTo));
            for (int i = vTFrom; i < vBTo; i++)
            {
                for (int j = hLTo; j < hRFrom; j++)
                {
                    if (j >= hLTo && j < hRFrom && ((i >= vTFrom && i < vTTo) || (i >= vBFrom && i < vBTo)))
                        vResult.Add((i, j));
                }
            }

            var hResult = new List<(int i, int j)>((vBFrom - vTTo) * (hRTo - hLFrom));
            for (int j = hLFrom; j < hRTo; j++)
            {
                for (int i = vTTo; i < vBFrom; i++)
                {
                    if (i >= vTTo && i < vBFrom && ((j >= hLFrom && j < hLTo) || (j >= hRFrom && j < hRTo)))
                        hResult.Add((i, j));
                }
            }

            return (v: [.. vResult], h: [.. hResult]);
        }

        public static (int vFrom, int vTo, int hFrom, int hTo) GetTargetUnitsRange(Vector2 center, int width, int height, int unitHeight, int unitWidth, int totalVCount, int totalHCount)
        {
            var (vFrom, vTo) = GetTargetVUnitsRange(center, height, unitHeight, totalVCount);
            var (hFrom, hTo) = GetTargetHUnitsRange(center, width, unitWidth, totalHCount);
            return (vFrom, vTo, hFrom, hTo);
        }

        public static (int vFrom, int vTo) GetTargetVUnitsRange(Vector2 center, int height, int unitHeight, int totalVCount)
        {
            int vFromPixel = (int)center.Y - height / 2;
            int vFrom = vFromPixel / unitHeight;
            if (vFrom < 0) vFrom = 0;
            if (vFrom > totalVCount) vFrom = totalVCount;

            int vToPixel = (int)center.Y + height / 2;
            int vTo = vToPixel / unitHeight;
            if (vTo * unitHeight < vToPixel) vTo += 1;
            if (vTo > totalVCount) vTo = totalVCount;
            if (vTo < 0) vTo = 0;
            return (vFrom, vTo);
        }

        public static (int hFrom, int hTo) GetTargetHUnitsRange(Vector2 center, int width, int unitWidth, int totalHCount)
        {
            int hFromPixel = (int)center.X - width / 2;
            int hFrom = hFromPixel / unitWidth;
            if (hFrom < 0) hFrom = 0;
            if (hFrom > totalHCount) hFrom = totalHCount;

            int hToPixel = (int)center.X + width / 2;
            int hTo = hToPixel / unitWidth;
            if (hTo * unitWidth < hToPixel) hTo += 1;
            if (hTo > totalHCount) hTo = totalHCount;
            if (hTo < 0) hTo = 0;
            return (hFrom, hTo);
        }

        public static Vector2 UpdateVelocityByGravity(Vector2 velocity, Vector2 gravityAcceleration, int? maxVerticalVelocity)
        {
            velocity += gravityAcceleration;

            if (maxVerticalVelocity != null && Math.Abs(velocity.Y) > maxVerticalVelocity)
                velocity.Y = (float)(Math.Sign(velocity.Y) * maxVerticalVelocity);

            return velocity;
        }

        public static Vector2 Move(Vector2 position, Vector2 velocity, float elapsedSeconds)
        {
            return position + velocity * elapsedSeconds;
        }

        public static Vector2 Move(Vector2 position, Vector2 velocity)
        {
            return position + velocity;
        }

        public static (Vector2 position, bool tCollision, bool bCollision, bool lCollision, bool rCollision) MoveBasedOnSurroundings(
                Vector2 currentPosition,
                Vector2 velocity,
                int collisionStep,
                Func<Vector2, Rectangle> getRectangle,
                Func<Vector2, CollisionRectangle> getCollisionRectangle,
                int margin, int unitHeight, int unitWidth, int totalVCount, int totalHCount,
                IUnit[,] units,
                float elapsedSeconds)
        {
            bool tCollision = false, bCollision = false, lCollision = false, rCollision = false;
            if (velocity != Vector2.Zero)
            {
                var frameVelocity = velocity * elapsedSeconds;
                var newPosition = Move(currentPosition, frameVelocity);
                var normalizedV = Vector2.Normalize(frameVelocity);
                Vector2 stepVelocity = normalizedV * collisionStep * elapsedSeconds;
                if (stepVelocity.LengthSquared() > frameVelocity.LengthSquared()) stepVelocity = frameVelocity;

                float? newPositionY = null, newPositionX = null;
                var rectangle = getRectangle(currentPosition);
                int rectWidth = rectangle.Width, rectHeight = rectangle.Height;
                var (vIndexes, hIndexes) = GetTargetVHPeripheralUnitsIndexes(rectangle.Center.ToVector2(),
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
                    var stepCollisionRectangle = getCollisionRectangle(Move(currentPosition, stepVelocity));

                    if (!bCollision && !tCollision)
                    {
                        foreach (var (vi, hi) in vIndexes)
                        {
                            IUnit? unit = units[vi, hi];
                            if (unit == null || !unit.IsStatic) continue;

                            var uRectangle = new CollisionRectangle(hi * unitWidth, vi * unitHeight, unitWidth, unitHeight);
                            if (!uRectangle.Intersects(stepCollisionRectangle)) continue;
                            if (stepVelocity.Y > 0 && uRectangle.Center.Y > stepCollisionRectangle.Center.Y)
                            {
                                newPositionY = newPositionY.HasValue ? Math.Min(newPositionY.Value, uRectangle.Top - rectHeight / 2) : uRectangle.Top - rectHeight / 2;
                                bCollision = true;
                            }

                            if (stepVelocity.Y < 0 && uRectangle.Center.Y < stepCollisionRectangle.Center.Y)
                            {
                                newPositionY = newPositionY.HasValue ? Math.Max(newPositionY.Value, uRectangle.Bottom + rectHeight / 2) : uRectangle.Bottom + rectHeight / 2;
                                tCollision = true;
                            }
                        }
                    }

                    if (!rCollision && !lCollision)
                    {
                        foreach (var (vi, hi) in hIndexes)
                        {
                            IUnit? unit = units[vi, hi];
                            if (unit == null || !unit.IsStatic) continue;

                            var uRectangle = new CollisionRectangle(hi * unitWidth, vi * unitHeight, unitWidth, unitHeight);
                            if (!uRectangle.Intersects(stepCollisionRectangle)) continue;
                            if (stepVelocity.X > 0 && uRectangle.Center.X > stepCollisionRectangle.Center.X)
                            {
                                newPositionX = newPositionX.HasValue ? Math.Min(newPositionX.Value, uRectangle.Left - rectWidth / 2) : uRectangle.Left - rectWidth / 2;
                                rCollision = true;
                            }
                            if (stepVelocity.X < 0 && uRectangle.Center.X < stepCollisionRectangle.Center.X)
                            {
                                newPositionX = newPositionX.HasValue ? Math.Max(newPositionX.Value, uRectangle.Right + rectWidth / 2) : uRectangle.Right + rectWidth / 2;
                                lCollision = true;
                            }
                        }
                    }
                    if ((newPositionY.HasValue && newPositionX.HasValue) || stepVelocity.LengthSquared() == frameVelocity.LengthSquared()) break;
                    stepVelocity += normalizedV * collisionStep * elapsedSeconds;
                    if (stepVelocity.LengthSquared() > frameVelocity.LengthSquared()) stepVelocity = frameVelocity;
                }

                if (newPositionY != null) newPosition.Y = (float)newPositionY;
                if (newPositionX != null) newPosition.X = (float)newPositionX;

                return (newPosition, tCollision, bCollision, lCollision, rCollision);
            }
            else
            {
                return (currentPosition, tCollision, bCollision, lCollision, rCollision);
            }
        }
    }
}