using System.Collections.Generic;

public class RewardInventory
{
    private readonly Dictionary<RewardType, int> rewardCounts = new();

    public IReadOnlyDictionary<RewardType, int> RewardCounts => rewardCounts;

    public void AddReward(Reward reward) {
        if (reward.RewardType == RewardType.BOMB) return;

        if (rewardCounts.ContainsKey(reward.RewardType)) {
            rewardCounts[reward.RewardType] += reward.Amount;
        } else {
            rewardCounts[reward.RewardType] = reward.Amount;
        }
    }

    public void RemoveReward(Reward reward) {
        if (!rewardCounts.ContainsKey(reward.RewardType)) return;

        rewardCounts[reward.RewardType] -= reward.Amount;

        if (rewardCounts[reward.RewardType] <= 0) {
            rewardCounts.Remove(reward.RewardType);
        }
    }

    public void Clear() {
        rewardCounts.Clear();
    }

    public Dictionary<RewardType, int> GetSnapshot() {
        return new Dictionary<RewardType, int>(rewardCounts);
    }

    public void RestoreSnapshot(Dictionary<RewardType, int> snapshot) {
        rewardCounts.Clear();
        foreach (var kvp in snapshot) {
            rewardCounts[kvp.Key] = kvp.Value;
        }
    }

    public int GetRewardAmount(RewardType type) {
        return rewardCounts.TryGetValue(type, out int amount) ? amount : 0;
    }
}