using NUnit.Framework;
using WormsApp.Data;
using WormsApp.domain.strategy;

namespace WormsAppTest
{
    public class NearestFoodStrategyTest
    {
        private Scene _scene;
        private IStrategy _strategy;

        [SetUp]
        public void Setup()
        {
            _scene = new Scene();
            _scene.Foods.Add(new Food(new Coordinates(0, 0), 100));
            _strategy = new NearestFoodStrategy();
        }
        
        [Test]
        public void TestMovingLeft()
        {
            var worm = new Worm(new Coordinates(1, 0), "Test", 10);
            _scene.Worms.Add(worm);
            var decisionIntent = _strategy.MakeDecision(worm, _scene);
            Assert.AreEqual(decisionIntent.Type, Intent.IntentType.Move);
            Assert.AreEqual(decisionIntent.Direction, Intent.MoveDirection.Left);
        }

        [Test]
        public void TestMovingRight()
        {
            var worm = new Worm(new Coordinates(-1, 0), "Test", 10);
            _scene.Worms.Add(worm);
            var decisionIntent = _strategy.MakeDecision(worm, _scene);
            Assert.AreEqual(decisionIntent.Type, Intent.IntentType.Move);
            Assert.AreEqual(decisionIntent.Direction, Intent.MoveDirection.Right);
        }

        [Test]
        public void TestMovingUp()
        {
            var worm = new Worm(new Coordinates(0, -1), "Test", 10);
            _scene.Worms.Add(worm);
            var decisionIntent = _strategy.MakeDecision(worm, _scene);
            Assert.AreEqual(decisionIntent.Type, Intent.IntentType.Move);
            Assert.AreEqual(decisionIntent.Direction, Intent.MoveDirection.Up);
        }

        [Test]
        public void TestMovingDown()
        {
            var worm = new Worm(new Coordinates(0, 1), "Test", 10);
            _scene.Worms.Add(worm);
            var decisionIntent = _strategy.MakeDecision(worm, _scene);
            Assert.AreEqual(decisionIntent.Type, Intent.IntentType.Move);
            Assert.AreEqual(decisionIntent.Direction, Intent.MoveDirection.Down);
        }
    }
}