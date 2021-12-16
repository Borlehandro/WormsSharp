using WormsApp.Data;

namespace WormsApp.domain.strategy
{
    public interface IStrategy
    {
        public Intent MakeDecision(Worm worm, Scene scene);
    }
}