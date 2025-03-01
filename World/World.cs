namespace GameApplication
{
    public class World
    {
        private readonly Unit[,] _world;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public World()
        {
            _world = new Unit[Constants.WorldVCount, Constants.WorldHCount];
            for (int i = 0; i < _world.GetLength(0); i++)
            {
                for (int j = 0; j < _world.GetLength(1); j++)
                {
                    _world[i, j] = new Unit();
                }
            }
            Width = _world.GetLength(1) * Constants.UnitWidth;
            Height = _world.GetLength(0) * Constants.UnitHeight;
        }
    }
}