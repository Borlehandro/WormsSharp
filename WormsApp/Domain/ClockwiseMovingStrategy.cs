using System.Collections.Generic;
using System.ComponentModel;

namespace WormsApp.Data
{
    public class ClockwiseMovingStrategy : IMovingStrategy
    {
        private const MoveIntent StartMoveIntent = MoveIntent.Up;

        public MoveIntent MakeMoveDecision(List<MoveIntent> movingHistory, Scene scene)
        {
            if (movingHistory.Count == 0) return StartMoveIntent;
            
            var previousMove = movingHistory[^1];
            return previousMove switch
            {
                MoveIntent.Up => MoveIntent.Right,
                MoveIntent.Right => MoveIntent.Down,
                MoveIntent.Down => MoveIntent.Left,
                MoveIntent.Left => MoveIntent.Up,
                MoveIntent.Nothing => StartMoveIntent,
                _ => throw new InvalidEnumArgumentException("Unsupported move intent:" + previousMove)
            };
        }
    }
}