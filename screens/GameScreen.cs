using System;

using Microsoft.Xna.Framework;

namespace GameApplication
{
    public class GameScreen(Game game) : Screen(game)
    {
        public override void Enter()
        {
            Console.WriteLine("GameScreen Enter");
        }

        public override void Initialize()
        {
            Console.WriteLine("GameScreen Initialize");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine("GameScreen LoadContent");
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
            Console.WriteLine("GameScreen Exit");
        }
    }
}