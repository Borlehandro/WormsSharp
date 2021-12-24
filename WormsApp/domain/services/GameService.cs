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

        public readonly Scene Scene;
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

            Scene = new Scene();
            Scene.Worms.Add(new Worm(new Coordinates(0, 0), "Test", StartEnergy));

            _clocks = new FixedTicksGameClocks(TicksToRun);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(Run);
            return Task.CompletedTask;
        }

        private void Run()
        {
            _clocks.Launch(NextTick);
        }

        public void NextTick(long tick)
        {
            Scene.Foods.RemoveAll(food => food.ExpiredAt <= tick);
            SpawnFood(tick);

            var wormsToAdd = new List<Worm>();

            Scene.Worms.ForEach(worm => { ApplyIntent(tick, worm, MakeDecision(worm), wormsToAdd); });
            Scene.Worms.RemoveAll(worm => worm.Energy <= 0);
            Scene.Worms.AddRange(wormsToAdd);

            _logger.Log(new GameState(Scene.Worms, Scene.Foods).ToString());

            if (tick == TicksToRun)
            {
                _logger.Dispose();
            }
        }

        private void SpawnFood(long tick)
        {
            _foodGenerator.GenerateFood(tick, Scene);
        }

        private Intent MakeDecision(Worm worm)
        {
            var intent = _decisionStrategy.MakeDecision(worm, Scene);
            worm.DecisionsHistory.Add(intent);
            return intent;
        }

        public void ApplyIntent(long tick, Worm worm, Intent intent, List<Worm> wormsToAdd)
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
                            Scene.Foods.FirstOrDefault(food => worm.Coordinates == food.Coordinates);
                        if (foodInCoordinates != null)
                        {
                            worm.Energy += FoodEnergy;
                            Scene.Foods.Remove(foodInCoordinates);
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
            return Scene.Foods.Any(food => food.Coordinates == coordinates);
        }

        private bool IsWorm(Coordinates coordinates)
        {
            return Scene.Worms.Any(worm => worm.Coordinates == coordinates);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}