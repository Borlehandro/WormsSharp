using System;
using NUnit.Framework;
using WormsApp.Data;
using WormsApp.Domain.Services;

namespace WormsAppTest
{
    public class FoodGenerationTests
    {
        [Test]
        public void TestGeneration()
        {
            var generator = new FoodGenerator();
            var scene = new Scene();
            generator.GenerateFood(1L, scene);
            Assert.Greater(scene.Foods.Count, 0);
            Assert.Pass();
        }
    }
}