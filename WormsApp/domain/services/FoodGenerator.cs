using System;
using System.Linq;
using WormsApp.Data;

namespace WormsApp.Domain.Services
{
    public class FoodGenerator
    {
        
        private const int FoodEnergy = 10;
        private const int FoodExpiredTicks = 11;
        
        public void GenerateFood(long tick, Scene scene)
        {
            Coordinates coordinates;
            do
            {
                coordinates = new Coordinates(
                    NormalRandomGenerator.NextNormal(new Random(), sigma: 5D),
                    NormalRandomGenerator.NextNormal(new Random(), sigma: 5D)
                );
            } while (IsFood(coordinates, scene));

            var worm = scene.Worms.FirstOrDefault(worm => worm.Coordinates == coordinates);
            if (worm != null)
            {
                worm.Energy += FoodEnergy;
            }
            else
            {
                scene.Foods.Add(new Food(coordinates, tick + FoodExpiredTicks));
            }
        }

        private static bool IsFood(Coordinates coordinates, Scene scene)
        {
            return scene.Foods.Any(food => food.Coordinates == coordinates);
        }
    }
}