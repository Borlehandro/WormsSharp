using System.IO;

namespace WormsApp.Domain.Services
{
    public class GameLauncher
    {
        private readonly StreamWriter  _gameOutput;
        private readonly IGameClocks _gameClocks;

        public GameLauncher(StreamWriter  gameOutput)
        {
            _gameOutput = gameOutput;
            _gameClocks = new FixedTicksGameClocks(100, new GameService(100, 100));
        }

        public void Start()
        {
            _gameClocks.Launch((tick, statistic) =>
            {
                _gameOutput.WriteLine(statistic.ToString());
            });
            _gameOutput.Close();
        }
    }
}