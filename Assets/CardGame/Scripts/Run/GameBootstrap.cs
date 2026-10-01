using System;
using CardGame.Core.Randomness;
using CardGame.Rewards.View;
using CardGame.Run.View;
using CardGame.Wheel;
using CardGame.Wheel.View;
using CardGame.Zones;
using CardGame.Zones.View;
using UnityEngine;

namespace CardGame.Run
{
    public class GameBootstrap : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private ZoneProgressionSO _progression;
        [SerializeField] private ZoneVisualsSO _visuals;
        [SerializeField] private GameRulesSO _rules;

        [Header("Views")]
        [SerializeField] private WheelView _wheelView;
        [SerializeField] private ZoneBarView _zoneBarView;
        [SerializeField] private RewardPanelView _rewardPanelView;
        [SerializeField] private BombPopupView _bombPopupView;
        [SerializeField] private BalanceBarView _balanceBarView;

        private GameRun _run;

        private void Awake() {
            var random = new SeededRandom(Environment.TickCount);
            var wallet = new Wallet(_rules.StartingBalances);
            var wheelBuilder = new WheelBuilder(_progression, new AmountCalculator(_progression), random);

            _run = new GameRun(_progression, _rules, wheelBuilder, new SpinResolver(random), wallet, new RevivePolicy(_rules));
            _wheelView.Initialize(_run, _visuals);
            _zoneBarView.Initialize(_run, _progression, _visuals);
            _rewardPanelView.Initialize(_run);
            _bombPopupView.Initialize(_run);
            _balanceBarView.Initialize(_run, wallet);
        }

        private void Start() {
            _run.StartNewRun();
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            if (_progression == null || _visuals == null || _rules == null
                || _wheelView == null || _zoneBarView == null || _rewardPanelView == null || _bombPopupView == null || _balanceBarView == null) {
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
