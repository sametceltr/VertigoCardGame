using CardGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Rewards.View
{
    public class RewardItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private ParentFitter _iconFitter;
        [SerializeField] private TextMeshProUGUI _totalText;

        public void Show(RewardDefinitionSO reward) {
            _icon.sprite = reward.Icon;
            _iconFitter.CalculateAspectRatio();
        }

        public void SetTotal(int total) {
            _totalText.text = AmountFormatter.Exact(total);
        }
    }
}
