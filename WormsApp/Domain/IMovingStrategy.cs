using System.Collections.Generic;

namespace WormsApp.Data
{
    public interface IMovingStrategy
    {
        public MoveIntent MakeMoveDecision(List<MoveIntent> movingHistory, Scene scene);
    }
}