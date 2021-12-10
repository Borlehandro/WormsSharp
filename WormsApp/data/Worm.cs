using System.Collections.Generic;
using WormsApp.Domain;

namespace WormsApp.Data
{
    public class Worm
    {
        public static int IdSequence = 0;

        // TODO: Remove brain
        private class Brain
        {
            private readonly List<Intent> _decisionsHistory = new();
            public IStrategy Strategy { get; }

            private readonly Worm _worm;

            public Brain(IStrategy strategy, Worm worm)
            {
                Strategy = strategy;
                _worm = worm;
            }

            public Intent MakeDecision(Scene scene)
            {
                var decision = Strategy.MakeDecision(_worm, _decisionsHistory, scene);
                _decisionsHistory.Add(decision);
                return decision;
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
            _brain = new Brain(strategy, this);
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