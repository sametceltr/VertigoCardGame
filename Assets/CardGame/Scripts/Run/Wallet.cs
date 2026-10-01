using System;
using System.Collections.Generic;
using CardGame.Rewards;

namespace CardGame.Run
{
    public class Wallet
    {
        private readonly Dictionary<RewardDefinitionSO, int> _balances = new Dictionary<RewardDefinitionSO, int>();

        public event Action<RewardDefinitionSO, int> BalanceChanged;

        public Wallet(IEnumerable<CurrencyAmount> startingBalances) {
            foreach (var balance in startingBalances) {
                _balances[balance.Currency] = balance.Amount;
            }
        }

        public int BalanceOf(RewardDefinitionSO currency) {
            return _balances.TryGetValue(currency, out int balance) ? balance : 0;
        }

        public void Add(RewardDefinitionSO currency, int amount) {
            _balances[currency] = BalanceOf(currency) + amount;
            BalanceChanged?.Invoke(currency, _balances[currency]);
        }

        public bool TrySpend(RewardDefinitionSO currency, int amount) {
            if (BalanceOf(currency) < amount) return false;

            _balances[currency] -= amount;
            BalanceChanged?.Invoke(currency, _balances[currency]);
            return true;
        }
    }
}
