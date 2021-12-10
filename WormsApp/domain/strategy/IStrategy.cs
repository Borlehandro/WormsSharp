using System.Collections.Generic;
using WormsApp.Data;

namespace WormsApp.Domain
{
    public interface IStrategy
    {
        public Intent MakeDecision(Worm worm, List<Intent> intentsHistory, Scene scene);
    }
}