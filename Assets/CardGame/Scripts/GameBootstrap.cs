using System;
using CardGame.Ads;
using CardGame.Core.Randomness;
using CardGame.Rewards.View;
using CardGame.Run;
using CardGame.Run.View;
using CardGame.Wheel;
using CardGame.Wheel.View;
using CardGame.Zones;
using CardGame.Zones.View;
using UnityEngine;

namespace CardGame
{
    public class GameBootstrap : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private ZoneProgressionSO _progression;
        [SerializeField] private ZoneVisualsSO _visuals;
        [SerializeField] private GameRulesSO _rules;

        [Header("Views")]
        [SerializeField] private CardGameView _cardGameView;
        [SerializeField] private WheelView _wheelView;
        [SerializeField] private ZoneBarView _zoneBarView;
        [SerializeField] private RewardPanelView _rewardPanelView;
        [SerializeField] private BombPopupView _bombPopupView;
        [SerializeField] private BalanceBarView _balanceBarView;
        [SerializeField] private MenuView _menuView;
        [SerializeField] private ConfirmPopupView _confirmPopupView;

        [Header("Services")]
        [SerializeField] private SimulatedAdService _adService;

        private GameRun _run;

        private void Awake() {
            var random = new SeededRandom(Environment.TickCount);
            var wallet = new Wallet(_rules.StartingBalances);
            var wheelBuilder = new WheelBuilder(_progression, new AmountCalculator(_progression), random);
            var collectedRewards = new CollectedRewards(wallet);
            var reviveOptions = new ReviveOptions(_rules.ReviveCurrency, _rules.ReviveCosts, wallet);

            _run = new GameRun(_progression, wheelBuilder, new SpinResolver(random), collectedRewards, reviveOptions, _adService);
            _cardGameView.Initialize(_run);
            _wheelView.Initialize(_run, _visuals);
            _zoneBarView.Initialize(_run, _progression, _visuals);
            _rewardPanelView.Initialize(_run, collectedRewards, _confirmPopupView);
            _bombPopupView.Initialize(_run, _confirmPopupView);
            _balanceBarView.Initialize(_run, wallet);
            _menuView.Initialize(_run);
            _confirmPopupView.Initialize(_run);
        }

        private void Start() {
            _run.EnterMenu();
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            if (_progression == null || _visuals == null || _rules == null || _cardGameView == null || _wheelView == null || _zoneBarView == null
                || _rewardPanelView == null || _bombPopupView == null || _balanceBarView == null || _menuView == null
                || _confirmPopupView == null || _adService == null) {
                Debug.LogWarning($"{name}: a data asset or view reference is missing.", this);
                return;
            }

            if (_wheelView.SliceCount != _progression.SliceCount) {
                Debug.LogWarning($"{name}: the wheel view has {_wheelView.SliceCount} slices but the progression expects {_progression.SliceCount}.", this);
            }
        }
    #endif
    }
}
