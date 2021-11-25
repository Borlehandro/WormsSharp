using System.Collections.Generic;

namespace WormsApp.Data
{
    public class Scene
    {
        private int _sizeX;
        private int _sizeY;

        public Scene(int sizeY, int sizeX)
        {
            _sizeY = sizeY;
            _sizeX = sizeX;
        }

        public List<Worm> Worms { get; } = new();
        public List<Food> Foods { get; } = new();
    }
}