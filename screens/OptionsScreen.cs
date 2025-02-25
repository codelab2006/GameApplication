using System;

using Microsoft.Xna.Framework;

namespace GameApplication
{
    public class OptionsScreen(Game game) : Screen(game)
    {
        public override void Enter()
        {
            Console.WriteLine("OptionsScreen Enter");
        }

        public override void Initialize()
        {
            Console.WriteLine("OptionsScreen Initialize");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine("OptionsScreen LoadContent");
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
            Console.WriteLine("OptionsScreen Exit");
        }
    }
}