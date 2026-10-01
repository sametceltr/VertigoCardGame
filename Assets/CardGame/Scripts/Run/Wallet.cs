using System;
using System.Collections.Generic;
using CardGame.Rewards;
using UnityEngine;

namespace CardGame.Run
{
    [System.Serializable]
    public class Wallet
    {
        [SerializeField] private int startingCoins = 100;
        [SerializeField] private int baseReviveCost = 25;

        private int currentBalance;
        private int currentReviveCost;

        private readonly Dictionary<RewardDefinitionSO, int> _balances = new Dictionary<RewardDefinitionSO, int>();

        public event Action<RewardDefinitionSO, int> BalanceChanged;

        public int CurrentBalance => currentBalance;
        public int ReviveCost => currentReviveCost;

        public Wallet() { }

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

        public void Initialize() {
            currentBalance = startingCoins;
            currentReviveCost = baseReviveCost;
            GameEvents.CoinBalanceChanged(currentBalance);
        }

        public bool CanAfford(int amount) {
            return currentBalance >= amount;
        }

        public bool TrySpendCoins(int amount) {
            if (!CanAfford(amount)) return false;

            currentBalance -= amount;
            GameEvents.CoinsSpent(amount);
            GameEvents.CoinBalanceChanged(currentBalance);
            return true;
        }

        public void AddCoins(int amount) {
            currentBalance += amount;
            GameEvents.CoinsEarned(amount);
            GameEvents.CoinBalanceChanged(currentBalance);
        }

        public bool TryRevive() {
            if (!TrySpendCoins(currentReviveCost)) return false;

            currentReviveCost *= 2;
            GameEvents.ReviveRequested();
            return true;
        }

        public void ResetReviveCost() {
            currentReviveCost = baseReviveCost;
        }
    }
}
