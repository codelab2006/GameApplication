using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameApplication
{
    internal class PlayerPart : Sprite
    {
        public PlayerPart() { }

        public PlayerPart(Texture2D texture2D, Vector2 position) : base(texture2D)
        {
            Position = position;
        }

        public virtual void Update(float elapsedSeconds, Vector2 velocity, (bool tCollision, bool bCollision, bool lCollision, bool rCollision) collisions) { }
    }

    internal class PlayerHead : PlayerPart
    {
        public PlayerHead() { }

        public PlayerHead(Texture2D texture2D, Vector2 position, PlayerBody playerBody) : base(texture2D, position)
        {
            Origin = new(Rectangle.Width / 2, Rectangle.Height);
            var bodyRectangle = playerBody.Rectangle;
            position.Y = bodyRectangle.Center.Y - bodyRectangle.Height / 2 + 1;
            Position = position;
        }
    }

    internal class PlayerBody : PlayerPart
    {
        public PlayerBody() { }

        public PlayerBody(Texture2D texture2D, Vector2 position) : base(texture2D, position)
        {
            Position = position;
        }
    }

    internal class PlayerLeftHand : PlayerPart
    {
        private int _direction = -1;
        private float _rotation = 0;
        private readonly float _maxRotation = MathHelper.ToRadians(20);

        public PlayerLeftHand() { }

        public PlayerLeftHand(Texture2D texture2D, Vector2 position, PlayerBody playerBody) : base(texture2D, position)
        {
            Origin = new(4, 0);
            var bodyRectangle = playerBody.Rectangle;
            position.X = bodyRectangle.Center.X + bodyRectangle.Width / 2 - 1;
            position.Y = playerBody.Rectangle.Top + 1;
            Position = position;
        }

        public override void Update(float elapsedSeconds, Vector2 velocity, (bool tCollision, bool bCollision, bool lCollision, bool rCollision) collisions)
        {
            if (!collisions.bCollision)
            {
                _direction = -1;
                _rotation = 0;
                Rotation = MathHelper.ToRadians(-160);
            }
            else
            {
                if (velocity.X == 0)
                {
                    _direction = -1;
                    _rotation = 0;
                    Rotation = _rotation;
                }
                else
                {
                    if (_rotation <= -_maxRotation)
                        _direction = 1;
                    if (_rotation >= _maxRotation)
                        _direction = -1;
                    _rotation += MathHelper.ToRadians(_direction * 180 * elapsedSeconds);
                    Rotation = _rotation;
                }
            }

            base.Update(elapsedSeconds, velocity, collisions);
        }
    }

    internal class PlayerRightHand : PlayerPart
    {
        private int _direction = 1;
        private float _rotation = 0;
        private readonly float _maxRotation = MathHelper.ToRadians(20);

        public PlayerRightHand() { }

        public PlayerRightHand(Texture2D texture2D, Vector2 position, PlayerBody playerBody) : base(texture2D, position)
        {
            Origin = new(4, 0);
            var bodyRectangle = playerBody.Rectangle;
            position.X = bodyRectangle.Center.X - bodyRectangle.Width / 2 + 1;
            position.Y = playerBody.Rectangle.Top + 1;
            Position = position;
        }

        public override void Update(float elapsedSeconds, Vector2 velocity, (bool tCollision, bool bCollision, bool lCollision, bool rCollision) collisions)
        {
            if (!collisions.bCollision)
            {
                _direction = 1;
                _rotation = 0;
                Rotation = MathHelper.ToRadians(-160);
            }
            else
            {
                if (velocity.X == 0)
                {
                    _direction = 1;
                    _rotation = 0;
                    Rotation = _rotation;
                }
                else
                {
                    if (_rotation >= _maxRotation)
                        _direction = -1;
                    if (_rotation <= -_maxRotation)
                        _direction = 1;
                    _rotation += MathHelper.ToRadians(_direction * 180 * elapsedSeconds);
                    Rotation = _rotation;
                }
            }

            base.Update(elapsedSeconds, velocity, collisions);
        }
    }

    internal class PlayerLeftLeg : PlayerPart
    {
        private int _direction = 1;
        private float _rotation = 0;
        private readonly float _maxRotation = MathHelper.ToRadians(20);

        public PlayerLeftLeg() { }

        public PlayerLeftLeg(Texture2D texture2D, Vector2 position, PlayerBody playerBody) : base(texture2D, position)
        {
            Origin = new(Rectangle.Width / 2, 0);
            var bodyRectangle = playerBody.Rectangle;
            position.X = bodyRectangle.Center.X + 1;
            position.Y = bodyRectangle.Center.Y + 2;
            Position = position;
        }

        public override void Update(float elapsedSeconds, Vector2 velocity, (bool tCollision, bool bCollision, bool lCollision, bool rCollision) collisions)
        {
            if (!collisions.bCollision)
            {
                _direction = 1;
                _rotation = 0;
                Rotation = MathHelper.ToRadians(-15);
            }
            else
            {
                if (velocity.X == 0)
                {
                    _direction = 1;
                    _rotation = 0;
                    Rotation = _rotation;
                }
                else
                {
                    if (_rotation >= _maxRotation)
                        _direction = -1;
                    if (_rotation <= -_maxRotation)
                        _direction = 1;
                    _rotation += MathHelper.ToRadians(_direction * 180 * elapsedSeconds);
                    Rotation = _rotation;
                }
            }

            base.Update(elapsedSeconds, velocity, collisions);
        }
    }

    internal class PlayerRightLeg : PlayerPart
    {
        private int _direction = -1;
        private float _rotation = 0;
        private readonly float _maxRotation = MathHelper.ToRadians(20);

        public PlayerRightLeg() { }

        public PlayerRightLeg(Texture2D texture2D, Vector2 position, PlayerBody playerBody) : base(texture2D, position)
        {
            Origin = new(Rectangle.Width / 2, 0);
            var bodyRectangle = playerBody.Rectangle;
            position.X = bodyRectangle.Center.X - 1;
            position.Y = bodyRectangle.Center.Y + 2;
            Position = position;
        }

        public override void Update(float elapsedSeconds, Vector2 velocity, (bool tCollision, bool bCollision, bool lCollision, bool rCollision) collisions)
        {
            if (!collisions.bCollision)
            {
                _direction = -1;
                _rotation = 0;
                Rotation = MathHelper.ToRadians(15);
            }
            else
            {
                if (velocity.X == 0)
                {
                    _direction = -1;
                    _rotation = 0;
                    Rotation = _rotation;
                }
                else
                {
                    if (_rotation <= -_maxRotation)
                        _direction = 1;
                    if (_rotation >= _maxRotation)
                        _direction = -1;
                    _rotation += MathHelper.ToRadians(_direction * 180 * elapsedSeconds);
                    Rotation = _rotation;
                }
            }

            base.Update(elapsedSeconds, velocity, collisions);
        }
    }

    public class Player : Sprite
    {
        private readonly RenderTarget2D _renderTarget2D;

        private readonly Vector2 _renderTarget2DOrigin;

        private PlayerHead _playerHead = new();
        private PlayerBody _playerBody = new();
        private PlayerLeftHand _playerLeftHand = new();
        private PlayerRightHand _playerRightHand = new();
        private PlayerLeftLeg _playerLeftLeg = new();
        private PlayerRightLeg _playerRightLeg = new();

        private Vector2 _velocity = Vector2.Zero;
        private bool _tCollision = false;
        private bool _bCollision = false;
        private bool _lCollision = false;
        private bool _rCollision = false;

        private bool _debugKeyDown = false;

        public Player() : base(null, new(0, 0, Constants.PlayerWidth, Constants.PlayerHeight))
        {
            _renderTarget2D = new(Global.GraphicsDevice,
                Constants.PlayerWidth + Constants.PlayerRenderTarget2DHMargin * 2,
                Constants.PlayerHeight + Constants.PlayerRenderTarget2DVMargin * 2
            );
            _renderTarget2DOrigin = new(_renderTarget2D.Width / 2, _renderTarget2D.Height / 2);
        }

        public void LoadContent()
        {
            Vector2 position = new(_renderTarget2D.Width / 2, _renderTarget2D.Height / 2);
            _playerBody = new(Global.Content.Load<Texture2D>("player/player-body"), position);
            _playerHead = new(Global.Content.Load<Texture2D>("player/player-head"), position, _playerBody);
            _playerLeftHand = new(Global.Content.Load<Texture2D>("player/player-hand-left"), position, _playerBody);
            _playerRightHand = new(Global.Content.Load<Texture2D>("player/player-hand-right"), position, _playerBody);
            _playerLeftLeg = new(Global.Content.Load<Texture2D>("player/player-leg-left"), position, _playerBody);
            _playerRightLeg = new(Global.Content.Load<Texture2D>("player/player-leg-right"), position, _playerBody);
        }

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

            if (state.IsKeyDown(Keys.Space))
            {
                var direction = Vector2.Zero - Vector2.UnitY;
                if (_bCollision)
                    _velocity += direction * Constants.InitialUpAcceleration;

                // if (true)
                // {
                //     _velocity += direction * Constants.InitialUpAcceleration;
                // }

                if (Math.Abs(_velocity.Y) > Constants.MaxVerticalVelocity) _velocity.Y = Math.Sign(_velocity.Y) * Constants.MaxVerticalVelocity;
            }

            if (_bCollision &&
                ((state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.D)) || (state.IsKeyDown(Keys.A) && state.IsKeyDown(Keys.D)) ||
                (_velocity.X > 0 && state.IsKeyDown(Keys.A)) || (_velocity.X < 0 && state.IsKeyDown(Keys.D))))
            {
                if (Math.Abs(_velocity.X) > 0) _velocity.X *= 1 - Constants.FrictionCoefficient;
                if (Math.Abs(_velocity.X) < 1) _velocity.X = 0;
            }
            else
            {
                var direction = Vector2.Zero;
                if (state.IsKeyDown(Keys.A)) direction -= Vector2.UnitX;
                if (state.IsKeyDown(Keys.D)) direction += Vector2.UnitX;
                _velocity += direction * Constants.InitialHorizontalAcceleration;
                if (Math.Abs(_velocity.X) > Constants.MaxHorizontalVelocity) _velocity.X = Math.Sign(_velocity.X) * Constants.MaxHorizontalVelocity;
            }

            // Console.WriteLine(_velocity);

            var elapsedSeconds = gameTime.GetElapsedSeconds();

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
                elapsedSeconds
            );

            _tCollision = tCollision;
            _bCollision = bCollision;
            _lCollision = lCollision;
            _rCollision = rCollision;

            Position = position;

            // Console.WriteLine($"{Position}, {_tCollision}, {_bCollision}, {_lCollision}, {_rCollision}");

            if ((_tCollision && _velocity.Y < 0) || (_bCollision && _velocity.Y > 0))
                _velocity.Y = 0;

            if ((_lCollision && _velocity.X < 0) || (_rCollision && _velocity.X > 0))
                _velocity.X = 0;

            _playerLeftHand.Update(elapsedSeconds, _velocity, (_tCollision, _bCollision, _lCollision, _rCollision));

            _playerLeftLeg.Update(elapsedSeconds, _velocity, (_tCollision, _bCollision, _lCollision, _rCollision));
            _playerRightLeg.Update(elapsedSeconds, _velocity, (_tCollision, _bCollision, _lCollision, _rCollision));

            _playerHead.Update(elapsedSeconds, _velocity, (_tCollision, _bCollision, _lCollision, _rCollision));

            _playerBody.Update(elapsedSeconds, _velocity, (_tCollision, _bCollision, _lCollision, _rCollision));

            _playerRightHand.Update(elapsedSeconds, _velocity, (_tCollision, _bCollision, _lCollision, _rCollision));
        }

        public void DrawTarget(SpriteBatch spriteBatch)
        {
            var graphicsDevice = Global.GraphicsDevice;
            graphicsDevice.SetRenderTarget(_renderTarget2D);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();

            _playerLeftHand.Draw(spriteBatch);

            _playerLeftLeg.Draw(spriteBatch);
            _playerRightLeg.Draw(spriteBatch);

            _playerHead.Draw(spriteBatch);

            _playerBody.Draw(spriteBatch);

            _playerRightHand.Draw(spriteBatch);

            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_renderTarget2D, Position, _renderTarget2D.Bounds, Color, Rotation, _renderTarget2DOrigin, Scale, Effects, LayerDepth);
        }
    }
}