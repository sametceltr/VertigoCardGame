using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardSlice : MonoBehaviour
{
    [SerializeField] RewardConfigSO rewardSO;
    [SerializeField] Image rewardIcon;
    [SerializeField] ParentFitter rewardIconFitter;
    [SerializeField] TextMeshProUGUI rewardAmount;
    private Reward _reward;

    public void UpdateSlice(Reward reward) {
        _reward = reward;
        UpdateVisual();
    }

    public void UpdateVisual() {
        var rewardConfig = rewardSO.GetConfig(_reward.RewardType);
        rewardIcon.sprite = rewardConfig.IconSprite;
        rewardIconFitter.CalculateAspectRatio();
        if (_reward.RewardType == RewardType.BOMB) {
            rewardAmount.text = "";
            return;
        }
        rewardAmount.text = PrepareText();
    }

    public string PrepareText() {
        float result = _reward.Amount / 1000;
        var division = Mathf.Floor(result);

        if (division > 0) {
            return "x" + division.ToString() + "K";
        } else {
            return "x" + _reward.Amount.ToString();
        }

    }
}