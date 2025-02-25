using System;

using Microsoft.Xna.Framework;

namespace GameApplication
{
    public class LogoScreen(Game game) : Screen(game)
    {
        public override void Enter()
        {
            Console.WriteLine("LogoScreen Enter");
        }

        public override void Initialize()
        {
            Console.WriteLine("LogoScreen Initialize");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine("LogoScreen LoadContent");
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

        public override void Exit()
        {
            Console.WriteLine("LogoScreen Exit");
        }
    }
}