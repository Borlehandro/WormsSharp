using System.Collections.Generic;

namespace WormsApp.Data
{
    public class Scene
    {
        public List<Worm> Worms { get; } = new();
        public List<Food> Foods { get; } = new();
    }
}