using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Rewards.View
{
    public class RewardItemView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountText;

        public void SetAmount(int amount) {
            amountText.text = amount.ToString();
        }
    }
}