using System;
using System.Collections.Generic;
using CardGame.Ads;
using CardGame.Wheel;
using CardGame.Zones;

namespace CardGame.Run
{
    public class GameRun
    {
        private readonly ZoneProgressionSO _progression;
        private readonly WheelBuilder _wheelBuilder;
        private readonly SpinResolver _spinResolver;
        private readonly CollectedRewards _collectedRewards;
        private readonly ReviveOptions _reviveOptions;
        private readonly IAdService _adService;

        private int _zone;
        private IReadOnlyList<WheelSlice> _slices;
        private int _landingIndex;

        public event Action<RunState> StateChanged;
        public event Action<ZoneInfo> ZoneChanged;
        public event Action<int> SpinStarted;

        public RunState State { get; private set; }
        public bool IsBombDisarmed { get; private set; }
        public int ReviveCost => _reviveOptions.GoldCost;
        public bool CanReviveWithGold => State == RunState.BombHit && _reviveOptions.CanAffordGold;
        public bool CanReviveWithAd => State == RunState.BombHit && _reviveOptions.IsAdAvailable;
        public bool CanLeave => State == RunState.Ready;
        public bool CanCollectRewards => CanLeave && _progression.GetZoneType(_zone) != ZoneType.Normal;

        public ExitOutcome ExitOutcome {
            get {
                if (CanCollectRewards) return ExitOutcome.CollectRewards;
                return _collectedRewards.HasAny ? ExitOutcome.LoseRewards : ExitOutcome.NoRewards;
            }
        }

        public GameRun(ZoneProgressionSO progression, WheelBuilder wheelBuilder, SpinResolver spinResolver,
            CollectedRewards collectedRewards, ReviveOptions reviveOptions, IAdService adService) {
            _progression = progression;
            _wheelBuilder = wheelBuilder;
            _spinResolver = spinResolver;
            _collectedRewards = collectedRewards;
            _reviveOptions = reviveOptions;
            _adService = adService;
        }

        public void EnterMenu() {
            SetState(RunState.InMenu);
        }

        public void StartNewRun() {
            if (State != RunState.InMenu) return;

            _reviveOptions.ResetForNewRun();
            EnterZone(ZoneProgressionSO.FirstZone);
        }

        public void Spin() {
            if (State != RunState.Ready) return;

            _landingIndex = _spinResolver.ResolveLandingIndex(_slices, IsBombDisarmed);
            SetState(RunState.Spinning);
            SpinStarted?.Invoke(_landingIndex);
        }

        public void ApplySpinResult() {
            if (State != RunState.Spinning) return;

            var slice = _slices[_landingIndex];
            if (slice.Type == SliceType.Bomb) {
                SetState(RunState.BombHit);
                return;
            }

            _collectedRewards.Add(slice.Reward, slice.Amount);
            if (_zone >= _progression.MaxZone) {
                _collectedRewards.DepositCurrencies();
                EndRun();
                return;
            }

            EnterZone(_zone + 1);
        }

        public void ReviveWithGold() {
            if (!CanReviveWithGold || !_reviveOptions.TryPayWithGold()) return;

            RespinWithBombDisarmed();
        }

        public void ReviveWithAd() {
            if (!CanReviveWithAd) return;

            SetState(RunState.WatchingAd);
            _adService.ShowRewardedAd(OnAdFinished);
        }

        private void OnAdFinished(bool isCompleted) {
            if (State != RunState.WatchingAd) return;

            if (!isCompleted) {
                SetState(RunState.BombHit);
                return;
            }

            _reviveOptions.UseAd();
            RespinWithBombDisarmed();
        }

        private void RespinWithBombDisarmed() {
            IsBombDisarmed = true;
            SetState(RunState.Ready);
        }

        public void GiveUp() {
            if (State != RunState.BombHit) return;

            EndRun();
        }

        public void Leave() {
            if (!CanLeave) return;

            if (CanCollectRewards) _collectedRewards.DepositCurrencies();
            EndRun();
        }

        private void EndRun() {
            _collectedRewards.Clear();
            EnterMenu();
        }

        private void EnterZone(int zone) {
            _zone = zone;
            _slices = _wheelBuilder.Build(zone);
            IsBombDisarmed = false;
            ZoneChanged?.Invoke(new ZoneInfo(zone, _progression.GetZoneType(zone), _slices));
            SetState(RunState.Ready);
        }

        private void SetState(RunState state) {
            State = state;
            StateChanged?.Invoke(state);
        }
    }
}
