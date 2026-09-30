using System;
using System.Collections.Generic;

namespace CardGame.Core.Randomness
{
    public static class WeightedPicker
    {
        public static int Pick(IReadOnlyList<int> weights, IRandom random) {
            int totalWeight = 0;
            for (int i = 0; i < weights.Count; i++) {
                totalWeight += weights[i];
            }

            if (totalWeight <= 0) throw new ArgumentException("At least one weight must be positive.", nameof(weights));

            int roll = random.Range(0, totalWeight);
            for (int i = 0; i < weights.Count; i++) {
                if (roll < weights[i]) return i;
                roll -= weights[i];
            }

            return weights.Count - 1;
        }
    }
}
