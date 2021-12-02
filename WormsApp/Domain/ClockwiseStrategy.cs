using System.Collections.Generic;
using System.ComponentModel;
using WormsApp.Data;
using WormsApp.Domain;

namespace WormsApp.Domain
{
    public class ClockwiseStrategy : IStrategy
    {
        private readonly Intent _startMoveIntent = new Intent(Intent.IntentType.Move, Intent.MoveDirection.Up);

        public Intent MakeMoveDecision(List<Intent> intentsHistory, Scene scene)
        {
            if (intentsHistory.Count == 0) return _startMoveIntent;

            var previousIntent = intentsHistory[^1];
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