using System;

using Microsoft.Xna.Framework;

namespace GameApplication
{
    public class MenuScreen(Game game) : Screen(game)
    {
        public override void Enter()
        {
            Console.WriteLine("MenuScreen Enter");
        }

        public override void Initialize()
        {
            Console.WriteLine("MenuScreen Initialize");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine("MenuScreen LoadContent");
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
            Console.WriteLine("MenuScreen Exit");
        }
    }
}