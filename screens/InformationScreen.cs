using System;

using Microsoft.Xna.Framework;

namespace GameApplication
{
    public class InformationScreen(Game game) : Screen(game)
    {
        public override void Enter()
        {
            Console.WriteLine("InformationScreen Enter");
        }

        public override void Initialize()
        {
            Console.WriteLine("InformationScreen Initialize");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine("InformationScreen LoadContent");
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
            Console.WriteLine("InformationScreen Exit");
        }
    }
}