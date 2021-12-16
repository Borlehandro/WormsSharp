using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using WormsApp.Data;
using WormsApp.domain.strategy;

namespace WormsApp.Domain.Services
{
    public class GameService : IHostedService
    {
        private const int TicksToRun = 100;

        public const int FoodEnergy = 10;

        private const int StartEnergy = 10;
        private const int MakeChildEnergy = 10;
        private const int TickEnergy = 1;

        private readonly Scene _scene;
        private readonly FoodGenerator _foodGenerator;
        private readonly IGameClocks _clocks;
        private readonly ILogger _logger;
        private readonly IStrategy _decisionStrategy;
        private readonly INamesGenerator _namesGenerator;

        public GameService(ILogger logger, FoodGenerator foodGenerator,
            IStrategy decisionStrategy, INamesGenerator namesGenerator)
        {
            _logger = logger;
            _foodGenerator = foodGenerator;
            _decisionStrategy = decisionStrategy;
            _namesGenerator = namesGenerator;

            _scene = new Scene();
            _scene.Worms.Add(new Worm(new Coordinates(0, 0), "Test", StartEnergy));

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
        }

        private void NextTick(long tick)
        {
            _scene.Foods.RemoveAll(food => food.ExpiredAt <= tick);
            SpawnFood(tick);

            var wormsToAdd = new List<Worm>();

            _scene.Worms.ForEach(worm => { ApplyIntent(tick, worm, MakeDecision(worm), wormsToAdd); });
            _scene.Worms.RemoveAll(worm => worm.Energy <= 0);
            _scene.Worms.AddRange(wormsToAdd);

            _logger.Log(new GameState(_scene.Worms, _scene.Foods).ToString());

            if (tick == TicksToRun)
            {
                _logger.Dispose();
            }
        }

        private void SpawnFood(long tick)
        {
            _foodGenerator.GenerateFood(tick, _scene);
        }

        private Intent MakeDecision(Worm worm)
        {
            var intent = _decisionStrategy.MakeDecision(worm, _scene);
            worm.DecisionsHistory.Add(intent);
            return intent;
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
                            _scene.Foods.FirstOrDefault(food => worm.Coordinates == food.Coordinates);
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
                    if (!IsFood(childCoordinates) && !IsWorm(childCoordinates) && worm.Energy >= MakeChildEnergy)
                    {
                        worm.Energy -= MakeChildEnergy;
                        wormsToAdd.Add(new Worm(childCoordinates, _namesGenerator.NextName(), StartEnergy));
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
            return _scene.Foods.Any(food => food.Coordinates == coordinates);
        }

        private bool IsWorm(Coordinates coordinates)
        {
            return _scene.Worms.Any(worm => worm.Coordinates == coordinates);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}