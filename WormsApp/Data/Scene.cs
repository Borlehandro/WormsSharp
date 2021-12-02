using System.Collections.Generic;

namespace WormsApp.Data
{
    public class Scene
    {
        public int SizeX { get; }
        public int SizeY { get; }

        public Scene(int sizeY, int sizeX)
        {
            SizeY = sizeY;
            SizeX = sizeX;
        }

        public List<Worm> Worms { get; } = new();
        public List<Food> Foods { get; } = new();
    }
}