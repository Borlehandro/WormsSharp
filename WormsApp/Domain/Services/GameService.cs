using System;
using System.Collections.Generic;
using System.Linq;
using WormsApp.Data;

namespace WormsApp.Domain.Services
{
    public class GameService
    {
        public const int FoodEnergy = 10;
        private const int FoodExpiredTicks = 11;

        private const int MakeChildEnergy = 10;
        private const int TickEnergy = 1;

        private readonly Scene _scene;

        public GameService()
        {
            _scene = new Scene();
            _scene.Worms.Add(new Worm(new Coordinates(0, 0), "Test", new NearestFoodStrategy()));
        }

        public GameState NextTick(long tick)
        {
            _scene.Foods.RemoveAll(food => food.ExpiredAt <= tick);
            SpawnFood(tick);
            var wormsToAdd = new List<Worm>();
            _scene.Worms.ForEach((worm) => { ApplyIntent(tick, worm, worm.MakeDecision(_scene), wormsToAdd); });
            _scene.Worms.RemoveAll((worm) => worm.Energy <= 0);
            _scene.Worms.AddRange(wormsToAdd);

            return new GameState(_scene.Worms, _scene.Foods);
        }

        private void SpawnFood(long tick)
        {
            Coordinates coordinates;
            do
            {
                coordinates = new Coordinates(
                    X: NormalRandomGenerator.NextNormal(new Random(), sigma: 5D),
                    Y: NormalRandomGenerator.NextNormal(new Random(), sigma: 5D)
                );
            } while (IsFood(coordinates));

            var worm = _scene.Worms.FirstOrDefault((worm) => worm.Coordinates == coordinates);
            if (worm != null)
            {
                worm.Energy += FoodEnergy;
            }
            else
            {
                _scene.Foods.Add(new Food(coordinates, tick + FoodExpiredTicks));
            }
        }

        private void ApplyIntent(long tick, Worm worm, Intent intent, List<Worm> wormsToAdd)
        {
            var (x, y) = worm.Coordinates;
            switch (intent.Type)
            {
                case Intent.IntentType.Move:
                    var newCoordinates = intent.Direction switch
                    {
                        Intent.MoveDirection.Right => new Coordinates(x + 1, y),
                        Intent.MoveDirection.Left => new Coordinates(x - 1, y),
                        Intent.MoveDirection.Up => new Coordinates(x, y + 1),
                        Intent.MoveDirection.Down => new Coordinates(x, y - 1),
                        _ => throw new ArgumentOutOfRangeException(nameof(intent.Direction), intent.Direction, null)
                    };
                    if (!IsWorm(newCoordinates))
                    {
                        worm.Coordinates = newCoordinates;
                        var foodInCoordinates =
                            _scene.Foods.FirstOrDefault((food) => worm.Coordinates == food.Coordinates);
                        if (foodInCoordinates != null)
                        {
                            worm.Energy += FoodEnergy;
                            _scene.Foods.Remove(foodInCoordinates);
                        }
                    }

                    worm.Energy -= TickEnergy;
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
                        worm.Energy -= MakeChildEnergy;
                        wormsToAdd.Add(new Worm(childCoordinates, "Worm#" + Worm.IdSequence++, worm.GetStrategy()));
                        return;
                    }

                    worm.Energy -= TickEnergy;
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