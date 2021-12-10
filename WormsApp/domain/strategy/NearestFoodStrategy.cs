using System;
using System.Collections.Generic;
using System.Linq;
using WormsApp.Data;
using WormsApp.Domain.Services;

namespace WormsApp.Domain
{
    public class NearestFoodStrategy : IStrategy
    {
        private List<Intent.MoveDirection> _currentWay = new();

        public Intent MakeDecision(Worm worm, List<Intent> intentsHistory, Scene scene)
        {
            if (worm.Energy >= 20)
            {
                return new Intent(Intent.IntentType.MakeChild, Intent.MoveDirection.Up);
            }
            
            if (_currentWay.Count > 0)
            {
                var intent = new Intent(Intent.IntentType.Move, _currentWay[0]);
                _currentWay.RemoveAt(0);
                return intent;
            }

            if (scene.Foods.Count > 0)
            {
                var minDistance = int.MaxValue;
                var nearestFood = scene.Foods[0];

                scene.Foods.ForEach(food =>
                {
                    var newDistance = DistanceBetween(worm.Coordinates, food.Coordinates);
                    if (newDistance >= minDistance) return;
                    minDistance = newDistance;
                    nearestFood = food;
                });

                if (minDistance > GameService.FoodEnergy)
                    return new Intent(Intent.IntentType.Nothing, Intent.MoveDirection.Up);
                
                _currentWay = BuildWay(worm.Coordinates, nearestFood.Coordinates);
                var intent = new Intent(Intent.IntentType.Move, _currentWay[0]);
                _currentWay.RemoveAt(0);
                return intent;

            }

            return new Intent(Intent.IntentType.Move, Intent.MoveDirection.Up);
        }

        private int DistanceBetween(Coordinates first, Coordinates second)
        {
            var (x1, y1) = second;
            var (x2, y2) = first;
            return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
        }

        private List<Intent.MoveDirection> BuildWay(Coordinates from, Coordinates to)
        {
            var (x1, y1) = from;
            var (x2, y2) = to;

            var directions = new List<Intent.MoveDirection>();
            directions.AddRange(Enumerable.Repeat(
                x1 < x2 ? Intent.MoveDirection.Right : Intent.MoveDirection.Left, Math.Abs(x2 - x1)
            ));
            directions.AddRange(Enumerable.Repeat(
                y1 < y2 ? Intent.MoveDirection.Up : Intent.MoveDirection.Down, Math.Abs(y2 - y1)
            ));
            return directions;
        }
    }
}