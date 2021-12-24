using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using WormsApp.Data;
using WormsApp.domain.services;
using WormsApp.Domain.Services;
using WormsApp.domain.strategy;

namespace WormsAppTest
{
    public class GameServiceTest
    {
        GameService _service = new(new SimpleLogger(StreamWriter.Null), new FoodGenerator(),
            new NearestFoodStrategy(), new SequenceNamesGenerator());

        [Test]
        [TestCase(Intent.MoveDirection.Down, 0, -1)]
        [TestCase(Intent.MoveDirection.Up, 0, 1)]
        [TestCase(Intent.MoveDirection.Left, -1, 0)]
        [TestCase(Intent.MoveDirection.Right, 1, 0)]
        public void TestMoving(Intent.MoveDirection direction, int xDelta, int yDelta)
        {
            List<Worm> wormsToAdd = new List<Worm>();
            var worm = _service.Scene.Worms[0];

            var prevEnergy = worm.Energy;
            var prevPosition = worm.Coordinates;

            Console.WriteLine(prevEnergy);
            Console.WriteLine(prevPosition);

            Console.WriteLine(worm.Energy);
            Console.WriteLine(worm.Coordinates);

            _service.ApplyIntent(0, worm, new Intent(Intent.IntentType.Move, direction), wormsToAdd);

            Assert.IsTrue(worm.Energy == prevEnergy - 1);
            Assert.IsTrue(worm.Coordinates.X == prevPosition.X + xDelta);
            Assert.IsTrue(worm.Coordinates.Y == prevPosition.Y + yDelta);
        }

        [Test]
        [TestCase(Intent.MoveDirection.Down, 0, -1)]
        [TestCase(Intent.MoveDirection.Up, 0, 1)]
        [TestCase(Intent.MoveDirection.Left, -1, 0)]
        [TestCase(Intent.MoveDirection.Right, 1, 0)]
        public void TestMovingOnFood(Intent.MoveDirection direction, int xDelta, int yDelta)
        {
            List<Worm> wormsToAdd = new List<Worm>();
            var worm = _service.Scene.Worms[0];

            _service.Scene.Foods.Add(
                new Food(
                    new Coordinates(worm.Coordinates.X + xDelta, worm.Coordinates.Y + yDelta),
                    1000)
            );

            var prevEnergy = worm.Energy;
            var prevPosition = worm.Coordinates;

            _service.ApplyIntent(0, worm, new Intent(Intent.IntentType.Move, direction), wormsToAdd);

            Console.WriteLine(prevEnergy);
            Console.WriteLine(prevPosition);

            Console.WriteLine(worm.Energy);
            Console.WriteLine(worm.Coordinates);

            Assert.IsTrue(worm.Energy == prevEnergy - 1 + GameService.FoodEnergy);
            Assert.IsTrue(worm.Coordinates.X == prevPosition.X + xDelta);
            Assert.IsTrue(worm.Coordinates.Y == prevPosition.Y + yDelta);
        }
    }
}