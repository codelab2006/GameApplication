using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameApplication
{
    public class Player : Sprite, ILightRenderer
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

        private bool _isFlipped = false;

        private bool _debugKeyDown = false;

        private MouseState _previousMouseState;

        private readonly Texture2D _lightTexture2D = CircleLight.NewInstance(128, Color.White);

        public Player() : base(null, new(0, 0, Constants.PlayerWidth, Constants.PlayerHeight))
        {
            _renderTarget2D = Global.GameGraphicsDevice.CreateRenderTarget2D(
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

            var currentMouseState = Mouse.GetState();
            var point = Global.Camera.ScreenToWorld(currentMouseState.X, currentMouseState.Y);
            if (point.X >= 0 && point.Y >= 0 && point.X < Global.World.Width && point.Y < Global.World.Height)
            {
                var rect = GetRectangleFByPosition(Position);
                var (vi, hi) = Global.GetTargetUnitIndex(point, Constants.UnitHeight, Constants.UnitWidth);
                if (!rect.Contains(point) && !rect.Intersects(new RectangleF(hi * Constants.UnitWidth, vi * Constants.UnitHeight, Constants.UnitWidth, Constants.UnitHeight)))
                {
                    if (currentMouseState.LeftButton == ButtonState.Pressed &&
                        _previousMouseState.LeftButton == ButtonState.Released)
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                            Global.World.SetUnitAt(vi, hi, new Unit(UnitBG.WOOD, vi, hi));
                        else
                            Global.World.SetUnitAt(vi, hi, new Unit(UnitFG.DIRT, vi, hi));

                    if (currentMouseState.RightButton == ButtonState.Pressed &&
                        _previousMouseState.RightButton == ButtonState.Released)
                        Global.World.SetUnitAt(vi, hi, null);
                }
            }
            _previousMouseState = currentMouseState;

            var state = Keyboard.GetState();

            _velocity = Global.UpdateVelocityByGravity(_velocity, Constants.GravityAcceleration, Constants.MaxVerticalVelocity);

            if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.W))
            {
                var direction = Vector2.Zero - Vector2.UnitY;
                if (_bCollision)
                    _velocity += direction * Constants.InitialUpAcceleration;

                if (state.IsKeyDown(Keys.W))
                {
                    _velocity += direction * Constants.ContinueUpAcceleration;
                }

                if (MathF.Abs(_velocity.Y) > Constants.MaxVerticalVelocity) _velocity.Y = MathF.Sign(_velocity.Y) * Constants.MaxVerticalVelocity;
            }

            if (_bCollision &&
                ((state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.D)) || (state.IsKeyDown(Keys.A) && state.IsKeyDown(Keys.D)) ||
                (_velocity.X > 0 && state.IsKeyDown(Keys.A)) || (_velocity.X < 0 && state.IsKeyDown(Keys.D))))
            {
                if (MathF.Abs(_velocity.X) > 0) _velocity.X *= 1 - Constants.FrictionCoefficient;
                if (MathF.Abs(_velocity.X) < 1) _velocity.X = 0;
            }
            else
            {
                var direction = Vector2.Zero;
                if (state.IsKeyDown(Keys.A)) direction -= Vector2.UnitX;
                if (state.IsKeyDown(Keys.D)) direction += Vector2.UnitX;
                _velocity += direction * Constants.InitialHorizontalAcceleration;
                if (MathF.Abs(_velocity.X) > Constants.MaxHorizontalVelocity) _velocity.X = MathF.Sign(_velocity.X) * Constants.MaxHorizontalVelocity;
            }

            if (_velocity.X > 0) _isFlipped = false;
            if (_velocity.X < 0) _isFlipped = true;

            Effects = _isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            var elapsedSeconds = gameTime.GetElapsedSeconds();

            var (position, tCollision, bCollision, lCollision, rCollision) = Global.MoveBasedOnSurroundings(
                Position,
                _velocity,
                Constants.CollisionStep,
                GetRectangleFByPosition,
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
            if (!Constants.ShowPlayer) return;
            var graphicsDevice = Global.GameGraphicsDevice;
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

        public void DrawLight(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(_lightTexture2D, position, _lightTexture2D.Bounds, Color.White, 0, _lightTexture2D.Bounds.Center.ToVector2(), 1, SpriteEffects.None, 0);
        }
    }
}