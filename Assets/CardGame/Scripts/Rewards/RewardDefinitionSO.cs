using UnityEngine;

namespace CardGame.Rewards
{
    [CreateAssetMenu(fileName = "RewardDefinition", menuName = "CardGame/Reward Definition")]
    public class RewardDefinitionSO : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private RewardCategory _category;

        public Sprite Icon => _icon;
        public RewardCategory Category => _category;
    }
}
