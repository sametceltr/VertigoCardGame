using System;
using CardGame.Rewards;
using UnityEngine;

namespace CardGame.Run
{
    [Serializable]
    public class CurrencyAmount
    {
        [SerializeField] private RewardDefinitionSO _currency;
        [SerializeField] private int _amount;

        public RewardDefinitionSO Currency => _currency;
        public int Amount => _amount;
    }
}
