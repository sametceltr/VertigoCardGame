using UnityEngine;

namespace CardGame.Run.View
{
    public class BalanceBarView : MonoBehaviour
    {
        [SerializeField] private CurrencyBalanceView[] _balanceViews;

        private GameRun _run;

        public void Initialize(GameRun run, Wallet wallet) {
            _run = run;
            foreach (var balanceView in _balanceViews) {
                balanceView.Initialize(wallet);
            }

            _run.StateChanged += OnStateChanged;
            gameObject.SetActive(false);
        }

        private void OnDestroy() {
            if (_run == null) return;

            _run.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(RunState state) {
            gameObject.SetActive(state == RunState.BombHit);
        }
    }
}
