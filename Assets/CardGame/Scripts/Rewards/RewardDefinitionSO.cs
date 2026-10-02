using UnityEngine;

namespace CardGame.Rewards
{
    [CreateAssetMenu(fileName = "RewardDefinition", menuName = "CardGame/Reward Definition")]
    public class RewardDefinitionSO : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private RewardCategory _category;

        [Header("Amount")]
        [SerializeField] private int _baseAmount;
        [SerializeField] private int _amountPerZone = 1;

        public Sprite Icon => _icon;
        public RewardCategory Category => _category;
        public int BaseAmount => _baseAmount;
        public int AmountPerZone => _amountPerZone;

    #if UNITY_EDITOR
        private void OnValidate() {
            if (_baseAmount < 0 || _amountPerZone < 0) Debug.LogWarning($"{name}: base amount and amount per zone can't be negative.", this);
            if (_baseAmount == 0 && _amountPerZone == 0) Debug.LogWarning($"{name}: base amount and amount per zone are both 0, so this reward pays nothing.", this);
        }
    #endif
    }
}
