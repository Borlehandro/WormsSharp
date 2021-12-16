using System.Collections.Generic;
using System.Text;

namespace WormsApp.Data
{
    public record GameState(List<Worm> Worms, List<Food> Foods)
    {
        public override string ToString()
        {
            var stringBuilder = new StringBuilder("Worms: [");
            Worms.ForEach(worm => stringBuilder.Append(worm + ", "));
            if (Worms.Count > 0)
                stringBuilder.Length -= 2;
            stringBuilder.Append("], Food[");
            Foods.ForEach(food => stringBuilder.Append(food + ","));
            if (Foods.Count > 0)
                stringBuilder.Length--;
            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }
    }
}