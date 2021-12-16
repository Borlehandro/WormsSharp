using System.Collections.Generic;

namespace WormsApp.Data
{
    public class Worm
    {

        public Coordinates Coordinates { get; set; }
        public int Energy;
        private readonly string _name;

        public readonly List<Intent> DecisionsHistory = new();

        public Worm(Coordinates coordinates, string name, int startEnergy)
        {
            Coordinates = coordinates;
            _name = name;
            Energy = startEnergy;
        }

        public override string ToString()
        {
            return _name + "-" + Energy + " " + Coordinates;
        }
    }
}