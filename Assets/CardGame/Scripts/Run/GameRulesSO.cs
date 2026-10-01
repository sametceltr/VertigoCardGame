using System.Collections.Generic;
using CardGame.Rewards;
using UnityEngine;

namespace CardGame.Run
{
    [CreateAssetMenu(fileName = "GameRules", menuName = "CardGame/Game Rules")]
    public class GameRulesSO : ScriptableObject
    {
        [SerializeField] private CurrencyAmount[] _startingBalances;
        [SerializeField] private RewardDefinitionSO _reviveCurrency;
        [SerializeField] private int[] _reviveCosts;

        public IReadOnlyList<CurrencyAmount> StartingBalances => _startingBalances;
        public RewardDefinitionSO ReviveCurrency => _reviveCurrency;
        public IReadOnlyList<int> ReviveCosts => _reviveCosts;

        private void OnValidate() {
            if (_reviveCurrency == null) Debug.LogWarning($"{name}: no revive currency is set.", this);
            if (_reviveCosts == null || _reviveCosts.Length == 0) Debug.LogWarning($"{name}: the revive cost table is empty.", this);

            if (_startingBalances == null) return;
            foreach (var balance in _startingBalances) {
                if (balance.Currency == null) Debug.LogWarning($"{name}: a starting balance has no currency.", this);
                else if (balance.Currency.Category != RewardCategory.Currency) Debug.LogWarning($"{name}: starting balance {balance.Currency.name} is not a currency.", this);
            }
        }
    }
}
