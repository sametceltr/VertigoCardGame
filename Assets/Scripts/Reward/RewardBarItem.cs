using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardBarItem : MonoBehaviour
{
    [SerializeField] private RewardConfigSO rewardConfig;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    private RewardType rewardType;
    private int currentAmount;

    public void Initialize(Reward reward) {
        rewardType = reward.RewardType;

        var config = rewardConfig.GetConfig(rewardType);
        icon.sprite = config.IconSprite;

        SetAmount(reward.Amount);
    }

    public void SetAmount(int amount) {
        currentAmount = amount;
        amountText.text = currentAmount.ToString();
    }

    public int GetAmount() => currentAmount;
    public RewardType GetRewardType() => rewardType;
}