using System.ComponentModel;
using WormsApp.Data;

namespace WormsApp.domain.strategy
{
    public class ClockwiseStrategy : IStrategy
    {
        private readonly Intent _startMoveIntent = new(Intent.IntentType.Move, Intent.MoveDirection.Up);

        public Intent MakeDecision(Worm worm, Scene scene)
        {
            if (worm.DecisionsHistory.Count == 0) return _startMoveIntent;

            var previousIntent = worm.DecisionsHistory[^1];
            return previousIntent.Direction switch
            {
                Intent.MoveDirection.Up => new Intent(Intent.IntentType.Move, Intent.MoveDirection.Right),
                Intent.MoveDirection.Right => new Intent(Intent.IntentType.Move, Intent.MoveDirection.Down),
                Intent.MoveDirection.Down => new Intent(Intent.IntentType.Move, Intent.MoveDirection.Left),
                Intent.MoveDirection.Left => new Intent(Intent.IntentType.Move, Intent.MoveDirection.Up),
                _ => throw new InvalidEnumArgumentException("Unsupported direction:" + previousIntent.Direction)
            };
        }
    }
}