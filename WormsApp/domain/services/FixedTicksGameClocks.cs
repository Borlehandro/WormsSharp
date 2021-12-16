namespace WormsApp.Domain.Services
{
    public class FixedTicksGameClocks : IGameClocks
    {

        private long _currentTick;
        private readonly long _ticksToRun;

        public FixedTicksGameClocks(long ticksToRun)
        {
            _ticksToRun = ticksToRun;
        }

        public void Launch(IGameClocks.OnTick onTickCallback)
        {
            while (_currentTick < _ticksToRun)
            {
                onTickCallback(++_currentTick);
            }
        }
    }
}