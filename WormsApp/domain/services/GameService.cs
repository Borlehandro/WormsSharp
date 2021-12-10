using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using WormsApp.Data;

namespace WormsApp.Domain.Services
{
    public class GameService : IHostedService
    {
        public const int TicksToRun = 100;

        public const int FoodEnergy = 10;
        private const int FoodExpiredTicks = 11;

        private const int MakeChildEnergy = 10;
        private const int TickEnergy = 1;

        private readonly IHostApplicationLifetime _appLifetime;
        private readonly Scene _scene;
        private readonly FoodGenerator _foodGenerator;
        private readonly IGameClocks _clocks;
        private readonly ILogger _logger;

        public GameService(ILogger logger, IHostApplicationLifetime appLifetime, FoodGenerator foodGenerator)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _foodGenerator = foodGenerator;
            _scene = new Scene();
            _scene.Worms.Add(new Worm(new Coordinates(0, 0), "Test", new NearestFoodStrategy()));

            _clocks = new FixedTicksGameClocks(TicksToRun);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TODO: Add cancellation support
            Task.Run(Run);
            return Task.CompletedTask;
        }

        private void Run()
        {
            _clocks.Launch(NextTick);
            _logger.Dispose();
        }

        private void NextTick(long tick)
        {
            _scene.Foods.RemoveAll(food => food.ExpiredAt <= tick);
            SpawnFood(tick);
            var wormsToAdd = new List<Worm>();
            _scene.Worms.ForEach((worm) => { ApplyIntent(tick, worm, worm.MakeDecision(_scene), wormsToAdd); });
            _scene.Worms.RemoveAll((worm) => worm.Energy <= 0);
            _scene.Worms.AddRange(wormsToAdd);

            _logger.Log(new GameState(_scene.Worms, _scene.Foods).ToString());
        }

        private void SpawnFood(long tick)
        {
            _foodGenerator.GenerateFood(tick, _scene);
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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}