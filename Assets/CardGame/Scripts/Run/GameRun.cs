using System;
using System.Collections.Generic;
using CardGame.Rewards;
using CardGame.Wheel;
using CardGame.Zones;

namespace CardGame.Run
{
    public class GameRun
    {
        private readonly ZoneProgressionSO _progression;
        private readonly GameRulesSO _rules;
        private readonly WheelBuilder _wheelBuilder;
        private readonly SpinResolver _spinResolver;
        private readonly Wallet _wallet;
        private readonly RevivePolicy _revivePolicy;
        private readonly Dictionary<RewardDefinitionSO, int> _collectedRewards = new Dictionary<RewardDefinitionSO, int>();

        private int _zone;
        private IReadOnlyList<WheelSlice> _slices;
        private int _landingIndex;
        private int _reviveCount;

        public event Action<RunState> StateChanged;
        public event Action<ZoneInfo> ZoneChanged;
        public event Action<int> SpinStarted;
        public event Action<RewardDefinitionSO, int, int> RewardCollected;
        public event Action RewardsCleared;

        public RunState State { get; private set; }
        public bool IsBombDisarmed { get; private set; }
        public IReadOnlyDictionary<RewardDefinitionSO, int> CollectedRewards => _collectedRewards;
        public int ReviveCost => _revivePolicy.CostFor(_reviveCount);
        public bool CanReviveWithGold => State == RunState.BombHit && _wallet.BalanceOf(_rules.ReviveCurrency) >= ReviveCost;

        public GameRun(ZoneProgressionSO progression, GameRulesSO rules, WheelBuilder wheelBuilder, SpinResolver spinResolver, Wallet wallet, RevivePolicy revivePolicy) {
            _progression = progression;
            _rules = rules;
            _wheelBuilder = wheelBuilder;
            _spinResolver = spinResolver;
            _wallet = wallet;
            _revivePolicy = revivePolicy;
        }

        public void EnterMenu() {
            SetState(RunState.InMenu);
        }

        public void StartNewRun() {
            if (State != RunState.InMenu) return;

            _reviveCount = 0;
            EnterZone(1);
        }

        public void Spin() {
            if (State != RunState.Ready) return;

            _landingIndex = _spinResolver.ResolveLandingIndex(_slices, IsBombDisarmed);
            SetState(RunState.Spinning);
            SpinStarted?.Invoke(_landingIndex);
        }

        public void CompleteSpin() {
            if (State != RunState.Spinning) return;

            var slice = _slices[_landingIndex];
            if (slice.Type == SliceType.Bomb) {
                SetState(RunState.BombHit);
                return;
            }

            Collect(slice);
            if (_zone >= _progression.MaxZone) {
                EndRun();
                return;
            }

            EnterZone(_zone + 1);
        }

        public void ReviveWithGold() {
            if (!CanReviveWithGold) return;

            _wallet.TrySpend(_rules.ReviveCurrency, ReviveCost);
            _reviveCount++;
            IsBombDisarmed = true;
            SetState(RunState.Ready);
        }

        public void GiveUp() {
            if (State != RunState.BombHit) return;

            EndRun();
        }

        private void EndRun() {
            _collectedRewards.Clear();
            RewardsCleared?.Invoke();
            EnterMenu();
        }

        private void EnterZone(int zone) {
            _zone = zone;
            _slices = _wheelBuilder.Build(zone);
            IsBombDisarmed = false;
            ZoneChanged?.Invoke(new ZoneInfo(zone, _progression.GetZoneType(zone), _slices));
            SetState(RunState.Ready);
        }

        private void Collect(WheelSlice slice) {
            _collectedRewards.TryGetValue(slice.Reward, out int total);
            total += slice.Amount;
            _collectedRewards[slice.Reward] = total;
            RewardCollected?.Invoke(slice.Reward, slice.Amount, total);
        }

        private void SetState(RunState state) {
            State = state;
            StateChanged?.Invoke(state);
        }
    }
}
