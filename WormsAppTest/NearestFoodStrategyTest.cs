using NUnit.Framework;
using WormsApp.Data;
using WormsApp.Domain.Services;
using WormsApp.domain.strategy;

namespace WormsAppTest
{
    public class NearestFoodStrategyTest
    {
        [Test]
        public void TestMovingLeft()
        {
            var scene = new Scene();
            scene.Foods.Add(new Food(new Coordinates(0, 0), 100));
            var strategy = new NearestFoodStrategy();
            var worm = new Worm(new Coordinates(1, 0), "Test", 10);
            scene.Worms.Add(worm);
            var decisionIntent = strategy.MakeDecision(worm, scene);
            Assert.AreEqual(decisionIntent.Type, Intent.IntentType.Move);
            Assert.AreEqual(decisionIntent.Direction, Intent.MoveDirection.Left);
            Assert.Pass();
        }

        [Test]
        public void TestMovingRight()
        {
            var scene = new Scene();
            scene.Foods.Add(new Food(new Coordinates(0, 0), 100));
            var strategy = new NearestFoodStrategy();
            var worm = new Worm(new Coordinates(-1, 0), "Test", 10);
            scene.Worms.Add(worm);
            var decisionIntent = strategy.MakeDecision(worm, scene);
            Assert.AreEqual(decisionIntent.Type, Intent.IntentType.Move);
            Assert.AreEqual(decisionIntent.Direction, Intent.MoveDirection.Right);
            Assert.Pass();
        }

        [Test]
        public void TestMovingUp()
        {
            var scene = new Scene();
            scene.Foods.Add(new Food(new Coordinates(0, 0), 100));
            var strategy = new NearestFoodStrategy();
            var worm = new Worm(new Coordinates(0, -1), "Test", 10);
            scene.Worms.Add(worm);
            var decisionIntent = strategy.MakeDecision(worm, scene);
            Assert.AreEqual(decisionIntent.Type, Intent.IntentType.Move);
            Assert.AreEqual(decisionIntent.Direction, Intent.MoveDirection.Up);
            Assert.Pass();
        }

        [Test]
        public void TestMovingDown()
        {
            var scene = new Scene();
            scene.Foods.Add(new Food(new Coordinates(0, 0), 100));
            var strategy = new NearestFoodStrategy();
            var worm = new Worm(new Coordinates(0, 1), "Test", 10);
            scene.Worms.Add(worm);
            var decisionIntent = strategy.MakeDecision(worm, scene);
            Assert.AreEqual(decisionIntent.Type, Intent.IntentType.Move);
            Assert.AreEqual(decisionIntent.Direction, Intent.MoveDirection.Down);
            Assert.Pass();
        }
    }
}