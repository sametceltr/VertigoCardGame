namespace CardGame.Core.Randomness
{
    public class SeededRandom : IRandom
    {
        private readonly System.Random _random;

        public SeededRandom(int seed) {
            _random = new System.Random(seed);
        }

        public int Range(int minInclusive, int maxExclusive) => _random.Next(minInclusive, maxExclusive);
    }
}
