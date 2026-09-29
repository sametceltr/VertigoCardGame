public class FixedRewardStrategy : IRewardGenerationStrategy
{
    private readonly Reward[] fixedRewards;

    public FixedRewardStrategy(Reward[] rewards) {
        fixedRewards = rewards;
    }

    public Reward[] GenerateRewards(Zone zone) {
        return fixedRewards;
    }
}