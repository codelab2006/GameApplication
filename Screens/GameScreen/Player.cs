using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameApplication
{
    public class Player(Texture2D? texture2D) : Sprite(texture2D)
    {
        private Vector2 _velocity = Vector2.Zero;
        private bool _tCollision = false;
        private bool _bCollision = false;
        private bool _lCollision = false;
        private bool _rCollision = false;

        private bool _debugKeyDown = false;

        private bool DebugPosition()
        {
            var state = Keyboard.GetState();
            if (state.IsKeyUp(Keys.Up) && state.IsKeyUp(Keys.Down) && state.IsKeyUp(Keys.Left) && state.IsKeyUp(Keys.Right))
                _debugKeyDown = false;
            if (!_debugKeyDown && (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Right)))
            {
                _velocity = Vector2.Zero;
                if (state.IsKeyDown(Keys.Up)) _velocity -= Vector2.UnitY;
                if (state.IsKeyDown(Keys.Down)) _velocity += Vector2.UnitY;
                if (state.IsKeyDown(Keys.Left)) _velocity -= Vector2.UnitX;
                if (state.IsKeyDown(Keys.Right)) _velocity += Vector2.UnitX;
                _debugKeyDown = true;
                Position = Global.Move(Position, _velocity, 1);
                _velocity = Vector2.Zero;
            }
            return _debugKeyDown;
        }

        public void Update(GameTime gameTime)
        {
            if (DebugPosition()) return;
            var state = Keyboard.GetState();

            _velocity = Global.UpdateVelocityByGravity(_velocity, Constants.GravityAcceleration, Constants.MaxVerticalVelocity);

            if ((state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.D)) || (state.IsKeyDown(Keys.A) && state.IsKeyDown(Keys.D)) ||
                (_velocity.X > 0 && state.IsKeyDown(Keys.A)) || (_velocity.X < 0 && state.IsKeyDown(Keys.D)))
            {
                if (Math.Abs(_velocity.X) > 0) _velocity.X *= 1 - Constants.FrictionCoefficient;
                if (Math.Abs(_velocity.X) < 1) _velocity.X = 0;
            }
            else
            {
                var direction = Vector2.Zero;
                if (state.IsKeyDown(Keys.W)) direction -= Vector2.UnitY;
                if (state.IsKeyDown(Keys.S)) direction += Vector2.UnitY;

                if (state.IsKeyDown(Keys.A)) direction -= Vector2.UnitX;
                if (state.IsKeyDown(Keys.D)) direction += Vector2.UnitX;
                _velocity += direction * Constants.InitialHorizontalAcceleration;
                if (Math.Abs(_velocity.Y) > Constants.MaxVerticalVelocity) _velocity.Y = Math.Sign(_velocity.Y) * Constants.MaxVerticalVelocity;
                if (Math.Abs(_velocity.X) > Constants.MaxHorizontalVelocity) _velocity.X = Math.Sign(_velocity.X) * Constants.MaxHorizontalVelocity;
            }

            Console.WriteLine(_velocity);

            var (position, tCollision, bCollision, lCollision, rCollision) = Global.MoveBasedOnSurroundings(
                Position,
                _velocity,
                Constants.CollisionStep,
                GetRectangleByPosition,
                GetCollisionRectangleByPosition,
                Constants.CollisionMargin,
                Constants.UnitHeight,
                Constants.UnitWidth,
                Constants.WorldVCount,
                Constants.WorldHCount,
                Global.World.Units,
                gameTime.GetElapsedSeconds()
            );

            _tCollision = tCollision;
            _bCollision = bCollision;
            _lCollision = lCollision;
            _rCollision = rCollision;

            Position = position;

            Console.WriteLine($"{Position}, {_tCollision}, {_bCollision}, {_lCollision}, {_rCollision}");

            if ((_tCollision && _velocity.Y < 0) || (_bCollision && _velocity.Y > 0))
                _velocity.Y = 0;

            if ((_lCollision && _velocity.X < 0) || (_rCollision && _velocity.X > 0))
                _velocity.X = 0;
        }
    }
}