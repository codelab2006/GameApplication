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
                Move(_velocity);
                _velocity = Vector2.Zero;
            }
            return _debugKeyDown;
        }

        public void Update(GameTime gameTime)
        {
            if (DebugPosition()) return;

            // _velocity = Global.UpdateVelocityByGravityAndElapsedSeconds(_velocity, Constants.GravityAcceleration, gameTime.GetElapsedSeconds(), Constants.MaxVerticalVelocity);

            var velocity = Vector2.Zero;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.D))
            {
                if (state.IsKeyDown(Keys.A)) velocity -= Vector2.UnitX;
                if (state.IsKeyDown(Keys.D)) velocity += Vector2.UnitX;
                _velocity += velocity * Constants.InitialHorizontalAcceleration * gameTime.GetElapsedSeconds();
                if (Math.Abs(_velocity.X) > Constants.MaxHorizontalVelocity) _velocity.X = Math.Sign(_velocity.X) * Constants.MaxHorizontalVelocity;
            }

            Console.WriteLine(_velocity);

            Move(_velocity);
        }

        private void Move(Vector2 velocity)
        {
            Position += velocity;
        }
    }
}