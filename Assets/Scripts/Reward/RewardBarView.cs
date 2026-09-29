using System.Collections.Generic;
using UnityEngine;

public class RewardBarView : MonoBehaviour
{
    [SerializeField] private RewardBarItem rewardItemPrefab;

    private readonly Dictionary<RewardType, RewardBarItem> rewardItems = new();

    public void UpdateReward(RewardType rewardType, int totalAmount) {
        if (rewardItems.TryGetValue(rewardType, out var item)) {
            item.SetAmount(totalAmount);
        } else {
            var newItem = Instantiate(rewardItemPrefab, transform);
            newItem.Initialize(new Reward(rewardType, 0, totalAmount));
            rewardItems.Add(rewardType, newItem);
        }
    }

    public void ClearAllRewards() {
        foreach (var item in rewardItems.Values) {
            Destroy(item.gameObject);
        }
        rewardItems.Clear();
    }
}