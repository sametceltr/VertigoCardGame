public class RNGRewardStrategy : IRewardGenerationStrategy
{
    private readonly ZoneRNG rngGenerator;

    public RNGRewardStrategy(ZoneRNG generator) {
        rngGenerator = generator;
    }

    public Reward[] GenerateRewards(Zone zone) {
        return rngGenerator.GenerateZoneRewards(zone);
    }
}