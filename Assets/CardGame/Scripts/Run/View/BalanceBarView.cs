using UnityEngine;

namespace CardGame.Run.View
{
    public class BalanceBarView : MonoBehaviour
    {
        [SerializeField] private CurrencyBalanceView[] _balanceViews;

        public void Initialize(Wallet wallet) {
            foreach (var balanceView in _balanceViews) {
                balanceView.Initialize(wallet);
            }
        }
    }
}
