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
                IUnit?[,] units,
                float elapsedSeconds)
        {
            bool tCollision = false, bCollision = false, lCollision = false, rCollision = false;
            if (velocity == Vector2.Zero || elapsedSeconds == 0) return (currentPosition, tCollision, bCollision, lCollision, rCollision);

            var frameVelocity = velocity * elapsedSeconds;
            if (frameVelocity == Vector2.Zero || float.IsNaN(frameVelocity.X) || float.IsNaN(frameVelocity.Y)) return (currentPosition, tCollision, bCollision, lCollision, rCollision);

            var normalizedV = Vector2.Normalize(frameVelocity);
            if (normalizedV == Vector2.Zero || float.IsNaN(normalizedV.X) || float.IsNaN(normalizedV.Y)) return (currentPosition, tCollision, bCollision, lCollision, rCollision);

            var newPosition = Move(currentPosition, frameVelocity);
            Vector2 stepVelocity = normalizedV * MathF.Max(collisionStep * elapsedSeconds, 0.5f);
            if (stepVelocity.LengthSquared() > frameVelocity.LengthSquared()) stepVelocity = frameVelocity;

            float? newPositionY = null, newPositionX = null;
            var rectangle = getRectangle(currentPosition);
            float rectWidth = rectangle.Width, rectHeight = rectangle.Height;
            var (vtIndexes, vbIndexes, hlIndexes, hrIndexes) = GetTargetVHPeripheralUnitsIndexes(rectangle.Center,
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
                var stepVCollisionRectangle = getRectangle(Move(currentPosition, new(0, stepVelocity.Y)));
                if (!bCollision && !tCollision)
                {
                    if (stepVelocity.Y > 0)
                    {
                        foreach (var (vi, hi) in vbIndexes)
                        {
                            IUnit? unit = units[vi, hi];
                            if (unit == null || !unit.IsStatic) continue;

                            var uRectangle = new RectangleF(hi * unitWidth, vi * unitHeight, unitWidth, unitHeight);
                            if (!uRectangle.Intersects(stepVCollisionRectangle)) continue;
                            if (uRectangle.Center.Y > stepVCollisionRectangle.Center.Y)
                            {
                                newPositionY = newPositionY.HasValue ? MathHelper.Min(newPositionY.Value, uRectangle.Top - (float)rectHeight / 2) : uRectangle.Top - (float)rectHeight / 2;
                                bCollision = true;
                            }
                        }
                    }
                    if (stepVelocity.Y < 0)
                    {
                        foreach (var (vi, hi) in vtIndexes)
                        {
                            IUnit? unit = units[vi, hi];
                            if (unit == null || !unit.IsStatic) continue;

                            var uRectangle = new RectangleF(hi * unitWidth, vi * unitHeight, unitWidth, unitHeight);
                            if (!uRectangle.Intersects(stepVCollisionRectangle)) continue;
                            if (uRectangle.Center.Y < stepVCollisionRectangle.Center.Y)
                            {
                                newPositionY = newPositionY.HasValue ? MathHelper.Max(newPositionY.Value, uRectangle.Bottom + (float)rectHeight / 2) : uRectangle.Bottom + (float)rectHeight / 2;
                                tCollision = true;
                            }
                        }
                    }
                }
                var stepHCollisionRectangle = getRectangle(Move(currentPosition, new(stepVelocity.X, 0)));
                if (!rCollision && !lCollision)
                {
                    List<(int vi, int hi, RectangleF uRectangle)> collisionRectangles = [];
                    if (stepVelocity.X > 0)
                    {
                        collisionRectangles = new(hrIndexes.Length);
                        foreach (var (vi, hi) in hrIndexes)
                        {
                            IUnit? unit = units[vi, hi];
                            if (unit == null || !unit.IsStatic) continue;

                            var uRectangle = new RectangleF(hi * unitWidth, vi * unitHeight, unitWidth, unitHeight);
                            if (!uRectangle.Intersects(stepHCollisionRectangle)) continue;
                            if (uRectangle.Center.X > stepHCollisionRectangle.Center.X)
                            {
                                var v = uRectangle.Left - (float)rectWidth / 2;
                                if (newPositionX.HasValue)
                                {
                                    if (newPositionX.Value >= v)
                                    {
                                        newPositionX = v;
                                        collisionRectangles.Add((vi, hi, uRectangle));
                                    }
                                }
                                else
                                {
                                    newPositionX = v;
                                    collisionRectangles.Add((vi, hi, uRectangle));
                                }
                                rCollision = true;
                            }
                        }
                    }
                    if (stepVelocity.X < 0)
                    {
                        collisionRectangles = new(hlIndexes.Length);
                        foreach (var (vi, hi) in hlIndexes)
                        {
                            IUnit? unit = units[vi, hi];
                            if (unit == null || !unit.IsStatic) continue;

                            var uRectangle = new RectangleF(hi * unitWidth, vi * unitHeight, unitWidth, unitHeight);
                            if (!uRectangle.Intersects(stepHCollisionRectangle)) continue;
                            if (uRectangle.Center.X < stepHCollisionRectangle.Center.X)
                            {
                                var v = uRectangle.Right + (float)rectWidth / 2;
                                if (newPositionX.HasValue)
                                {
                                    if (newPositionX.Value <= v)
                                    {
                                        newPositionX = v;
                                        collisionRectangles.Add((vi, hi, uRectangle));
                                    }
                                }
                                else
                                {
                                    newPositionX = v;
                                    collisionRectangles.Add((vi, hi, uRectangle));
                                }
                                lCollision = true;
                            }
                        }
                    }
                    if (bCollision && collisionRectangles.Count == 1)
                    {
                        (int _, int _, RectangleF lastCollisionRectangle) = collisionRectangles[^1];
                        if (lastCollisionRectangle.Bottom == rectangle.Bottom && lastCollisionRectangle.Center.Y > rectangle.Center.Y)
                        {
                            if (rCollision &&
                                TryAdjustPosition(
                                    direction: +1,
                                    lastCollisionRectangle,
                                    rectHeight,
                                    unitWidth,
                                    unitHeight,
                                    getRectangle,
                                    vtIndexes,
                                    units,
                                    ref newPositionX,
                                    ref newPositionY,
                                    ref rCollision)) break;
                            if (lCollision &&
                                TryAdjustPosition(
                                    direction: -1,
                                    lastCollisionRectangle,
                                    rectHeight,
                                    unitWidth,
                                    unitHeight,
                                    getRectangle,
                                    vtIndexes,
                                    units,
                                    ref newPositionX,
                                    ref newPositionY,
                                    ref lCollision)) break;
                        }
                    }
                }
                if ((newPositionY.HasValue && newPositionX.HasValue) || stepVelocity.LengthSquared() == frameVelocity.LengthSquared()) break;
                stepVelocity += normalizedV * collisionStep * elapsedSeconds;
                if (stepVelocity.LengthSquared() > frameVelocity.LengthSquared()) stepVelocity = frameVelocity;
            }

            if (!tCollision && !bCollision && !lCollision && !rCollision)
            {
                HandleDiagonalCollision(currentPosition, newPosition, getRectangle, unitHeight, unitWidth, totalVCount, totalHCount, units,
                               ref lCollision,
                               ref rCollision,
                               ref newPositionX);
                if (newPositionX != null) newPosition.X = MathF.Floor((float)newPositionX);
            }
            else
            {
                if (newPositionY != null) newPosition.Y = MathF.Floor((float)newPositionY);
                if (newPositionX != null) newPosition.X = MathF.Floor((float)newPositionX);
            }

            return (newPosition, tCollision, bCollision, lCollision, rCollision);
        }

        private static void HandleDiagonalCollision(
            Vector2 currentPosition,
            Vector2 newPosition,
            Func<Vector2, RectangleF> getRectangle,
            int unitHeight,
            int unitWidth,
            int totalVCount,
            int totalHCount,
            IUnit?[,] units,
            ref bool lcollisionFlag,
            ref bool rcollisionFlag,
            ref float? newPositionX)
        {
            var rectangle = getRectangle(newPosition);
            float rectWidth = rectangle.Width;
            var (vFrom, vTo, hFrom, hTo) = GetTargetUnitsRange(rectangle.Center,
                rectWidth,
                rectangle.Height,
                unitHeight,
                unitWidth,
                totalVCount,
                totalHCount);
            var xOffset = newPosition.X - currentPosition.X;
            var yOffset = newPosition.Y - currentPosition.Y;
            var directions = new (bool condition, int vi, int hi, bool isRight)[]
            {
                (yOffset > 0 && xOffset > 0 && !(rectangle.Right > totalHCount * unitWidth || rectangle.Bottom > totalVCount * unitHeight), vTo - 1, hTo - 1, true),
                (yOffset > 0 && xOffset < 0 && !(rectangle.Left < 0 || rectangle.Bottom > totalVCount * unitHeight), vTo - 1, hFrom, false),
                (yOffset < 0 && xOffset < 0 && !(rectangle.Left < 0 || rectangle.Top < 0), vFrom, hFrom, false),
                (yOffset < 0 && xOffset > 0 && !(rectangle.Right > totalHCount * unitWidth || rectangle.Top < 0), vFrom, hTo - 1, true),
            };

            foreach (var (condition, vi, hi, isRight) in directions)
            {
                if (!condition) continue;
                if (vi < 0 || vi >= units.GetLength(0) || hi < 0 || hi >= units.GetLength(1))
                    continue;
                IUnit? unit = units[vi, hi];
                if (unit == null || !unit.IsStatic) continue;
                var uRectangle = new RectangleF(hi * unitWidth, vi * unitHeight, unitWidth, unitHeight);
                if (isRight)
                {
                    rcollisionFlag = true;
                    newPositionX = uRectangle.Left - rectWidth / 2f;
                }
                else
                {
                    lcollisionFlag = true;
                    newPositionX = uRectangle.Right + rectWidth / 2f;
                }
                break;
            }
        }

        private static bool TryAdjustPosition(
            int direction,
            RectangleF lastCollisionRectangle,
            float rectHeight,
            float unitWidth,
            float unitHeight,
            Func<Vector2, RectangleF> getRectangle,
            (int vi, int hi)[] vtIndexes,
            IUnit?[,] units,
            ref float? newPositionX,
            ref float? newPositionY,
            ref bool collisionFlag)
        {
            if (!newPositionX.HasValue) return false;

            var tempNewPositionY = lastCollisionRectangle.Top - rectHeight / 2f;
            var tempNewPositionX = newPositionX.Value + direction;
            var tempVCollisionRectangle = getRectangle(new Vector2(tempNewPositionX, tempNewPositionY));
            var tempTCollision = false;

            foreach (var (vi, hi) in vtIndexes)
            {
                IUnit? unit = units[vi, hi];
                if (unit == null || !unit.IsStatic) continue;

                var uRectangle = new RectangleF(hi * unitWidth, vi * unitHeight, unitWidth, unitHeight);
                if (!uRectangle.Intersects(tempVCollisionRectangle)) continue;

                if (uRectangle.Center.Y < tempVCollisionRectangle.Center.Y)
                {
                    tempTCollision = true;
                    break;
                }
            }

            if (!tempTCollision)
            {
                newPositionY = tempNewPositionY;
                newPositionX = tempNewPositionX;
                collisionFlag = false;
                return true;
            }

            return false;
        }

        private static ((int v, int h)[] vt, (int v, int h)[] vb, (int v, int h)[] hl, (int v, int h)[] hr) GetTargetVHPeripheralUnitsIndexes(Vector2 center, float width, float height, int margin, int unitHeight, int unitWidth, int totalVCount, int totalHCount)
        {
            var (vTFrom, vTTo, vBFrom, vBTo, hLFrom, hLTo, hRFrom, hRTo) = GetTargetPeripheralUnitsRange(center, width, height, margin, unitHeight, unitWidth, totalVCount, totalHCount);

            var vTResult = new List<(int i, int j)>((hRTo - hLFrom) * (vTTo - vTFrom));
            var vBResult = new List<(int i, int j)>((hRTo - hLFrom) * (vBTo - vBFrom));
            for (int i = vTFrom; i < vBTo; i++)
            {
                for (int j = hLFrom; j < hRTo; j++)
                {
                    if (i >= vTFrom && i < vTTo)
                        vTResult.Add((i, j));
                    if (i >= vBFrom && i < vBTo)
                        vBResult.Add((i, j));
                }
            }

            var hLResult = new List<(int i, int j)>((vBTo - vTFrom) * (hLTo - hLFrom));
            var hRResult = new List<(int i, int j)>((vBTo - vTFrom) * (hRTo - hRFrom));
            for (int j = hLFrom; j < hRTo; j++)
            {
                for (int i = vTFrom; i < vBTo; i++)
                {
                    if (j >= hLFrom && j < hLTo)
                        hLResult.Add((i, j));
                    if (j >= hRFrom && j < hRTo)
                        hRResult.Add((i, j));
                }
            }

            return (vt: [.. vTResult], vb: [.. vBResult], hl: [.. hLResult], hr: [.. hRResult]);
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

        public static (int vi, int hi) GetTargetUnitIndex(Vector2 p, float unitHeight, float unitWidth)
        {
            return ((int)(p.Y / unitHeight), (int)(p.X / unitWidth));
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
            float fromPixelF = center - size / 2;
            int fromPixel = (int)fromPixelF;
            int from = (int)(fromPixel / unitSize);
            if (from < 0) from = 0;
            if (from > totalCount) from = totalCount;

            float toPixelF = center + size / 2;
            int toPixel = (int)toPixelF;
            int to = (int)(toPixel / unitSize);
            if ((to * unitSize) < toPixelF) to += 1;
            if (to < 0) to = 0;
            if (to > totalCount) to = totalCount;

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