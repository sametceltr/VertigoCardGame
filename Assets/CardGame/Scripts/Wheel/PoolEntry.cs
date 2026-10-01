using System;
using CardGame.Rewards;
using UnityEngine;

namespace CardGame.Wheel
{
    [Serializable]
    public class PoolEntry
    {
        [SerializeField] private RewardDefinitionSO _reward;
        [SerializeField] private int _appearanceWeight;

        public RewardDefinitionSO Reward => _reward;
        public int AppearanceWeight => _appearanceWeight;
    }
}
