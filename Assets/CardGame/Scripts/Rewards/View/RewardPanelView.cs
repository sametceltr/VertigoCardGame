using System.Collections.Generic;
using CardGame.Run;
using UnityEngine;

namespace CardGame.Rewards.View
{
    public class RewardPanelView : MonoBehaviour
    {
        [Header("List")]
        [SerializeField] private RectTransform _itemContainer;
        [SerializeField] private RewardItemView _itemPrefab;

        private readonly Dictionary<RewardDefinitionSO, RewardItemView> _items = new Dictionary<RewardDefinitionSO, RewardItemView>();

        private GameRun _run;

        public void Initialize(GameRun run) {
            _run = run;
            _run.RewardCollected += OnRewardCollected;
            _run.RewardsCleared += OnRewardsCleared;
        }

        private void OnDestroy() {
            if (_run == null) return;

            _run.RewardCollected -= OnRewardCollected;
            _run.RewardsCleared -= OnRewardsCleared;
        }

        private void OnRewardCollected(RewardDefinitionSO reward, int amount, int total) {
            ItemFor(reward).SetTotal(total);
        }

        private void OnRewardsCleared() {
            foreach (var item in _items.Values) {
                Destroy(item.gameObject);
            }
            _items.Clear();
        }

        private RewardItemView ItemFor(RewardDefinitionSO reward) {
            if (_items.TryGetValue(reward, out var item)) return item;

            item = Instantiate(_itemPrefab, _itemContainer);
            item.Show(reward);
            _items.Add(reward, item);
            return item;
        }
    }
}
