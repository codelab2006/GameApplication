using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public abstract class Screen(Game game) : DrawableGameComponent(game)
    {
        protected SpriteBatch _spriteBatch = new(game.GraphicsDevice);

        public virtual void Enter()
        {
            Console.WriteLine($"{GetType().Name} Enter");
        }

        public override void Initialize()
        {
            Console.WriteLine($"{GetType().Name} Initialize");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine($"{GetType().Name} LoadContent");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public virtual void Exit()
        {
            Console.WriteLine($"{GetType().Name} Exit");
        }
    }
}