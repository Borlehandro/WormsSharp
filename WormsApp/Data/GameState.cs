﻿using System.Collections.Generic;
using System.Text;

namespace WormsApp.Data
{
    public record GameState(List<Worm> Worms)
    {
        public override string ToString()
        {
            var stringBuilder = new StringBuilder("Worms: [");
            Worms.ForEach((worm) => stringBuilder.Append(worm.ToString()));
            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }
    }
}