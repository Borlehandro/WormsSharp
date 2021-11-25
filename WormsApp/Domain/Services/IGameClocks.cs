using WormsApp.Data;

namespace WormsApp.Domain.Services
{
    public interface IGameClocks
    {

        public delegate void OnTick(long currentTick, GameState state);

        public void Launch(OnTick onTickCallback);
    }
}