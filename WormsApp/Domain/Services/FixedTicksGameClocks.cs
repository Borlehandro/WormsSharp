using System;

namespace WormsApp.Domain.Services
{
    public class FixedTicksGameClocks : IGameClocks
    {
        private readonly GameService _gameService;

        private long _currentTick;
        private readonly long _ticksToRun;

        public FixedTicksGameClocks(long ticksToRun, GameService gameService)
        {
            _ticksToRun = ticksToRun;
            _gameService = gameService;
        }

        public void Launch(IGameClocks.OnTick onTickCallback)
        {
            while (_currentTick < _ticksToRun)
            {
                onTickCallback(++_currentTick, _gameService.NextTick());
            }
        }
    }
}