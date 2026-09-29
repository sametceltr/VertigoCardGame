using System.Collections.Generic;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    [SerializeField] private RewardBarView rewardBarView;

    private RewardInventory inventory;
    private Stack<IRewardCommand> commandHistory = new();

    private void Awake() {
        inventory = new RewardInventory();
    }

    private void OnEnable() {
        GameEvents.OnRewardAwarded += HandleRewardAwarded;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable() {
        GameEvents.OnRewardAwarded -= HandleRewardAwarded;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private void HandleRewardAwarded(Reward reward) {
        if (reward.RewardType == RewardType.BOMB) return;

        var command = new CollectRewardCommand(inventory, reward);
        ExecuteCommand(command);

        rewardBarView.UpdateReward(reward.RewardType, inventory.GetRewardAmount(reward.RewardType));
    }

    private void HandleGameOver() {
        var command = new ClearRewardsCommand(inventory);
        ExecuteCommand(command);

        rewardBarView.ClearAllRewards();
    }

    private void ExecuteCommand(IRewardCommand command) {
        command.Execute();
        commandHistory.Push(command);
    }

    public void UndoLastCommand() {
        if (commandHistory.Count > 0) {
            var command = commandHistory.Pop();
            command.Undo();
        }
    }
}