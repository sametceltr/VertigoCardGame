namespace CardGame.Core.Randomness
{
    public interface IRandom
    {
        int Range(int minInclusive, int maxExclusive);
    }
}
