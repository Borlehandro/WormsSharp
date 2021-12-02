using System.Collections.Generic;
using WormsApp.Domain;

namespace WormsApp.Data
{
    public class Worm
    {
        // TODO: Remove brain
        private class Brain
        {
            private readonly List<Intent> _decisionsHistory = new();
            public IStrategy Strategy { get; }

            public Brain(IStrategy strategy)
            {
                Strategy = strategy;
            }

            public Intent MakeDecision(Scene scene)
            {
                var moveDecision = Strategy.MakeMoveDecision(_decisionsHistory, scene);
                _decisionsHistory.Add(moveDecision);
                return moveDecision;
            }
        }

        private readonly Brain _brain;
        public Coordinates Coordinates { get; set; }
        public int Energy;
        public string Name { get; }

        public Worm(Coordinates coordinates, string name, IStrategy strategy)
        {
            Coordinates = coordinates;
            Name = name;
            Energy = 10;
            _brain = new Brain(strategy);
        }

        public Intent MakeDecision(Scene scene)
        {
            return _brain.MakeDecision(scene);
        }

        public IStrategy GetStrategy()
        {
            return _brain.Strategy;
        }

        public override string ToString()
        {
            return Name + "-" + Energy + " " + Coordinates;
        }
    }
}