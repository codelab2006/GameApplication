using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameApplication
{
    public class Player(Texture2D? texture2D) : Sprite(texture2D)
    {
        private Vector2 _velocity = Vector2.Zero;

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
                Move(_velocity, 1);
                _velocity = Vector2.Zero;
            }
            return _debugKeyDown;
        }

        public void Update(GameTime gameTime)
        {
            if (DebugPosition()) return;

            // _velocity = Global.UpdateVelocityByGravityAndElapsedSeconds(_velocity, Constants.GravityAcceleration, Constants.MaxVerticalVelocity);

            var direction = Vector2.Zero;
            var state = Keyboard.GetState();
            if ((state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.D)) || (state.IsKeyDown(Keys.A) && state.IsKeyDown(Keys.D)))
            {
                if (Math.Abs(_velocity.X) > 0) _velocity.X *= 1 - Constants.FrictionCoefficient;
                if (Math.Abs(_velocity.X) < 1) _velocity.X = 0;
            }
            else
            {
                if (state.IsKeyDown(Keys.A)) direction -= Vector2.UnitX;
                if (state.IsKeyDown(Keys.D)) direction += Vector2.UnitX;
                _velocity += direction * Constants.InitialHorizontalAcceleration;
                if (Math.Abs(_velocity.X) > Constants.MaxHorizontalVelocity) _velocity.X = Math.Sign(_velocity.X) * Constants.MaxHorizontalVelocity;
            }
            Move(_velocity, gameTime.GetElapsedSeconds());
        }

        private void Move(Vector2 velocity, float elapsedSeconds)
        {
            Position += velocity * elapsedSeconds;
        }
    }
}