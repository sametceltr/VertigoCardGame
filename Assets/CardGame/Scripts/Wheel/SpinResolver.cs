using System.Collections.Generic;
using CardGame.Core.Randomness;

namespace CardGame.Wheel
{
    public class SpinResolver
    {
        private readonly IRandom _random;

        public SpinResolver(IRandom random) {
            _random = random;
        }

        public int ResolveLandingIndex(IReadOnlyList<WheelSlice> slices, bool isBombDisarmed) {
            var weights = new int[slices.Count];
            for (int i = 0; i < slices.Count; i++) {
                bool skip = isBombDisarmed && slices[i].Type == SliceType.Bomb;
                weights[i] = skip ? 0 : slices[i].LandingWeight;
            }

            return WeightedPicker.Pick(weights, _random);
        }
    }
}
