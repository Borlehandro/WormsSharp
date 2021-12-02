using System.Collections.Generic;
using WormsApp.Data;

namespace WormsApp.Domain
{
    public interface IStrategy
    {
        public Intent MakeMoveDecision(List<Intent> intentsHistory, Scene scene);
    }
}