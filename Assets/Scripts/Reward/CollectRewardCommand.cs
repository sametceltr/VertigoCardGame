public class CollectRewardCommand : IRewardCommand
{
    private readonly RewardInventory inventory;
    private readonly Reward reward;

    public CollectRewardCommand(RewardInventory inv, Reward r) {
        inventory = inv;
        reward = r;
    }

    public void Execute() {
        inventory.AddReward(reward);
    }

    public void Undo() {
        inventory.RemoveReward(reward);
    }
}
