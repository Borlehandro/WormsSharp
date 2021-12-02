using System.Collections.Generic;
using System.Text;

namespace WormsApp.Data
{
    public record GameState(List<Worm> Worms, List<Food> Foods)
    {
        public override string ToString()
        {
            var stringBuilder = new StringBuilder("Worms: [");
            Worms.ForEach((worm) => stringBuilder.Append(worm + ","));
            stringBuilder.Length--;
            stringBuilder.Append("], Food[");
            Foods.ForEach((food) => stringBuilder.Append(food + ","));
            stringBuilder.Length--;
            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }
    }
}