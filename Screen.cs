using Microsoft.Xna.Framework;

namespace GameApplication
{
    public abstract class Screen(Game game) : DrawableGameComponent(game)
    {
        public abstract void Enter();
        public abstract void Exit();
    }
}