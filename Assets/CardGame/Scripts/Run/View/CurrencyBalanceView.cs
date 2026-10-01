using CardGame.Rewards;
using CardGame.UI;
using TMPro;
using UnityEngine;

namespace CardGame.Run.View
{
    public class CurrencyBalanceView : MonoBehaviour
    {
        [SerializeField] private RewardDefinitionSO _currency;
        [SerializeField] private TextMeshProUGUI _amountText;

        private Wallet _wallet;

        public void Initialize(Wallet wallet) {
            _wallet = wallet;
            _wallet.BalanceChanged += OnBalanceChanged;
            ShowBalance(_wallet.BalanceOf(_currency));
        }

        private void OnDestroy() {
            if (_wallet == null) return;

            _wallet.BalanceChanged -= OnBalanceChanged;
        }

        private void OnBalanceChanged(RewardDefinitionSO currency, int balance) {
            if (currency == _currency) ShowBalance(balance);
        }

        private void ShowBalance(int balance) {
            _amountText.text = AmountFormatter.Exact(balance);
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            if (_currency != null && _currency.Category != RewardCategory.Currency) {
                Debug.LogWarning($"{name}: {_currency.name} is not a currency.", this);
            }
        }
    #endif
    }
}
