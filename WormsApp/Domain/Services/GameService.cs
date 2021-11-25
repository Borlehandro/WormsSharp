using System;
using WormsApp.Data;

namespace WormsApp.Domain.Services
{
    public class GameService
    {
        private readonly Scene _scene;
        
        private static Coordinates MoveCoordinates(Coordinates coordinates, MoveIntent intent)
        {
            var (x, y) = coordinates;
            return intent switch
            {
                MoveIntent.Right => new Coordinates(x + 1, y),
                MoveIntent.Left => new Coordinates(x - 1, y),
                MoveIntent.Up => new Coordinates(x, y + 1),
                MoveIntent.Down => new Coordinates(x, y - 1),
                MoveIntent.Nothing => new Coordinates(x, y),
                _ => throw new ArgumentOutOfRangeException(nameof(intent), intent, null)
            };
        }

        public GameService(int initialSizeX, int initialSizeY)
        {
            _scene = new Scene(initialSizeX, initialSizeY);
            _scene.Worms.Add(new Worm(new Coordinates(0, 0), "Test", new ClockwiseMovingStrategy()));
        }

        public GameState NextTick()
        {
            _scene.Worms.ForEach((worm) =>
            {
                worm.Coordinates = MoveCoordinates(worm.Coordinates, worm.MakeMoveDecision(_scene));
            });

            return new GameState(_scene.Worms);
        }
    }
}