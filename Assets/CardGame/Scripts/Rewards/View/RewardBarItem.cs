using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardBarItem : MonoBehaviour
{
    [SerializeField] private RewardConfigSO rewardConfig;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Initialize(Reward reward) {
        var config = rewardConfig.GetConfig(reward.RewardType);
        icon.sprite = config.IconSprite;

        SetAmount(reward.Amount);
    }

    public void SetAmount(int amount) {
        amountText.text = amount.ToString();
    }
}