using System;
using System.Collections.Generic;
using System.Linq;
using WormsApp.Data;

namespace WormsApp.Domain.Services
{
    public class GameService
    {
        private readonly Scene _scene;

        public GameService(int initialSizeX, int initialSizeY)
        {
            _scene = new Scene(initialSizeX, initialSizeY);
            _scene.Worms.Add(new Worm(new Coordinates(0, 0), "Test", new ClockwiseStrategy()));
        }

        public GameState NextTick(long tick)
        {
            Console.Out.WriteLine("Pre Spawn: " + new GameState(_scene.Worms, _scene.Foods));
            _scene.Foods.RemoveAll(food => food.ExpiredAt <= tick);
            SpawnFood(tick);
            Console.Out.WriteLine("Post Spawn: " + new GameState(_scene.Worms, _scene.Foods));
            var wormsToAdd = new List<Worm>();
            _scene.Worms.ForEach((worm) => { ApplyIntent(tick, worm, worm.MakeDecision(_scene), wormsToAdd); });
            _scene.Worms.RemoveAll((worm) => worm.Energy <= 0);
            _scene.Worms.AddRange(wormsToAdd);
            Console.Out.WriteLine("Post Move: " + new GameState(_scene.Worms, _scene.Foods));

            return new GameState(_scene.Worms, _scene.Foods);
        }

        private void SpawnFood(long tick)
        {
            Coordinates coordinates;
            do
            {
                coordinates = new Coordinates(
                    X: NormalRandomGenerator.NextNormal(new Random()) % _scene.SizeX,
                    Y: NormalRandomGenerator.NextNormal(new Random()) % _scene.SizeY
                );
            } while (IsFood(coordinates));

            var worm = _scene.Worms.FirstOrDefault((worm) => worm.Coordinates == coordinates);
            if (worm != null)
            {
                // TODO: Fix hardcoded food energy
                worm.Energy += 5;
            }
            else
            {
                // TODO: Fix hardcoded duration
                _scene.Foods.Add(new Food(coordinates, tick + 11L));
            }
        }

        private void ApplyIntent(long tick, Worm worm, Intent intent, List<Worm> wormsToAdd)
        {
            var (x, y) = worm.Coordinates;
            switch (intent.Type)
            {
                case Intent.IntentType.Move:
                    worm.Coordinates = intent.Direction switch
                    {
                        Intent.MoveDirection.Right => new Coordinates(x + 1, y),
                        Intent.MoveDirection.Left => new Coordinates(x - 1, y),
                        Intent.MoveDirection.Up => new Coordinates(x, y + 1),
                        Intent.MoveDirection.Down => new Coordinates(x, y - 1),
                        _ => throw new ArgumentOutOfRangeException(nameof(intent.Direction), intent.Direction, null)
                    };
                    var foodInCoordinates = _scene.Foods.FirstOrDefault((food) => worm.Coordinates == food.Coordinates);
                    if (foodInCoordinates != null)
                    {
                        // TODO: Fix hardcoded food energy
                        worm.Energy += 5;
                        _scene.Foods.Remove(foodInCoordinates);
                    }

                    // TODO: Fix hardcoded energy
                    worm.Energy -= 1;
                    break;
                case Intent.IntentType.MakeChild:
                    var childCoordinates = intent.Direction switch
                    {
                        Intent.MoveDirection.Right => new Coordinates(x + 1, y),
                        Intent.MoveDirection.Left => new Coordinates(x - 1, y),
                        Intent.MoveDirection.Up => new Coordinates(x, y + 1),
                        Intent.MoveDirection.Down => new Coordinates(x, y - 1),
                        _ => throw new ArgumentOutOfRangeException(nameof(intent.Direction), intent.Direction, null)
                    };
                    if (!IsFood(childCoordinates) && !IsWorm(childCoordinates) && worm.Energy >= 10)
                    {
                        // TODO: Fix hardcoded food energy
                        worm.Energy -= 10;
                        wormsToAdd.Add(new Worm(childCoordinates, worm.Name + "Child" + tick, worm.GetStrategy()));
                        return;
                    }

                    // TODO: Fix hardcoded food energy
                    worm.Energy -= 1;
                    break;

                case Intent.IntentType.Nothing:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsFood(Coordinates coordinates)
        {
            return _scene.Foods.Any((food) => food.Coordinates == coordinates);
        }

        private bool IsWorm(Coordinates coordinates)
        {
            return _scene.Worms.Any((worm) => worm.Coordinates == coordinates);
        }
    }
}