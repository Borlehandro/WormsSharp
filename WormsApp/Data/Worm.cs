using System.Collections.Generic;

namespace WormsApp.Data
{
    public class Worm
    {
        // TODO: Remove brain
        private class Brain
        {
            private readonly List<MoveIntent> _movingHistory = new();
            private readonly IMovingStrategy _strategy;

            public Brain(IMovingStrategy strategy)
            {
                _strategy = strategy;
            }

            public MoveIntent MakeMoveDecision(Scene scene)
            {
                var moveDecision = _strategy.MakeMoveDecision(_movingHistory, scene);
                _movingHistory.Add(moveDecision);
                return moveDecision;
            }
        }

        private readonly Brain _brain;
        public Coordinates Coordinates { get; set; }
        private string Name { get; }

        public Worm(Coordinates coordinates, string name, IMovingStrategy movingStrategy)
        {
            Coordinates = coordinates;
            Name = name;
            _brain = new Brain(movingStrategy);
        }

        public MoveIntent MakeMoveDecision(Scene scene)
        {
            return _brain.MakeMoveDecision(scene);
        }

        public override string ToString()
        {
            return "{ " + "name:\"" + Name + "\", " + "x:" + Coordinates.X + ", " + "y:" + Coordinates.Y + " }";
        }
    }
}