using System;
using System.Collections.Generic;
using CardGame.Rewards;

namespace CardGame.Run
{
    public class CollectedRewards
    {
        private readonly Wallet _wallet;
        private readonly Dictionary<RewardDefinitionSO, int> _totals = new Dictionary<RewardDefinitionSO, int>();

        public event Action<RewardDefinitionSO, int> Changed;
        public event Action Cleared;

        public IReadOnlyDictionary<RewardDefinitionSO, int> Totals => _totals;
        public bool HasAny => _totals.Count > 0;

        public CollectedRewards(Wallet wallet) {
            _wallet = wallet;
        }

        public void Add(RewardDefinitionSO reward, int amount) {
            _totals.TryGetValue(reward, out int total);
            total += amount;
            _totals[reward] = total;
            Changed?.Invoke(reward, total);
        }

        public void DepositCurrencies() {
            foreach (var collected in _totals) {
                if (collected.Key.Category == RewardCategory.Currency) _wallet.Add(collected.Key, collected.Value);
            }
        }

        public void Clear() {
            _totals.Clear();
            Cleared?.Invoke();
        }
    }
}
