using System.Collections.Generic;
using CardGame.Run;
using CardGame.Run.View;
using CardGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Rewards.View
{
    public class RewardPanelView : MonoBehaviour
    {
        private const string LeaveButtonName = "ui_button_exit";

        [Header("List")]
        [SerializeField] private RectTransform _itemContainer;
        [SerializeField] private RewardItemView _itemPrefab;

        [Header("Buttons")]
        [SerializeField] private Button _leaveButton;

        private readonly Dictionary<RewardDefinitionSO, RewardItemView> _items = new Dictionary<RewardDefinitionSO, RewardItemView>();

        private GameRun _run;
        private CollectedRewards _collectedRewards;

        public void Initialize(GameRun run, CollectedRewards collectedRewards, ConfirmPopupView confirmPopup) {
            _run = run;
            _collectedRewards = collectedRewards;
            _collectedRewards.Changed += OnRewardChanged;
            _collectedRewards.Cleared += OnRewardsCleared;
            _run.StateChanged += OnStateChanged;
            _leaveButton.onClick.AddListener(() => confirmPopup.Ask(_run.ExitOutcome, _run.Leave));
        }

        private void OnDestroy() {
            if (_run == null) return;

            _collectedRewards.Changed -= OnRewardChanged;
            _collectedRewards.Cleared -= OnRewardsCleared;
            _run.StateChanged -= OnStateChanged;
        }

        private void OnRewardChanged(RewardDefinitionSO reward, int total) {
            ItemFor(reward).SetTotal(total);
        }

        private void OnRewardsCleared() {
            foreach (var item in _items.Values) {
                Destroy(item.gameObject);
            }
            _items.Clear();
        }

        private void OnStateChanged(RunState state) {
            _leaveButton.interactable = _run.CanLeave;
        }

        private RewardItemView ItemFor(RewardDefinitionSO reward) {
            if (_items.TryGetValue(reward, out var item)) return item;

            item = Instantiate(_itemPrefab, _itemContainer);
            item.Show(reward);
            _items.Add(reward, item);
            return item;
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            _leaveButton = ChildButtons.FindIfMissing(this, _leaveButton, LeaveButtonName);
        }
    #endif
    }
}
