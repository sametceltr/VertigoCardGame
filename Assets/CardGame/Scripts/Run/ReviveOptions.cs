using System;
using System.Collections.Generic;
using CardGame.Rewards;

namespace CardGame.Run
{
    public class ReviveOptions
    {
        private readonly RewardDefinitionSO _goldCurrency;
        private readonly IReadOnlyList<int> _goldCosts;
        private readonly Wallet _wallet;

        private int _goldRevives;
        private bool _isAdUsed;

        public int GoldCost => _goldCosts[Math.Min(_goldRevives, _goldCosts.Count - 1)];
        public bool CanAffordGold => _wallet.BalanceOf(_goldCurrency) >= GoldCost;
        public bool IsAdAvailable => !_isAdUsed;

        public ReviveOptions(RewardDefinitionSO goldCurrency, IReadOnlyList<int> goldCosts, Wallet wallet) {
            _goldCurrency = goldCurrency;
            _goldCosts = goldCosts;
            _wallet = wallet;
        }

        public bool TryPayWithGold() {
            if (!_wallet.TrySpend(_goldCurrency, GoldCost)) return false;

            _goldRevives++;
            return true;
        }

        public void UseAd() {
            _isAdUsed = true;
        }

        public void ResetForNewRun() {
            _goldRevives = 0;
        }
    }
}
