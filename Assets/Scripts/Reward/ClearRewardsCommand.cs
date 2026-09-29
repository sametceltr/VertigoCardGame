using System.Collections.Generic;

public class ClearRewardsCommand : IRewardCommand
{
    private readonly RewardInventory inventory;
    private Dictionary<RewardType, int> previousState;

    public ClearRewardsCommand(RewardInventory inv) {
        inventory = inv;
    }

    public void Execute() {
        previousState = inventory.GetSnapshot();
        inventory.Clear();
    }

    public void Undo() {
        if (previousState != null) {
            inventory.RestoreSnapshot(previousState);
        }
    }
}