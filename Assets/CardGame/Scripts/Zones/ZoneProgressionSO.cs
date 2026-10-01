using System.Collections.Generic;
using CardGame.Rewards;
using CardGame.Wheel;
using UnityEngine;

namespace CardGame.Zones
{
    [CreateAssetMenu(fileName = "ZoneProgression", menuName = "CardGame/Zone Progression")]
    public class ZoneProgressionSO : ScriptableObject
    {
        public const int FirstZone = 1;

        [Header("Zones")]
        [SerializeField] private int _maxZone;
        [SerializeField] private int _safeZoneInterval;
        [SerializeField] private int _superZoneInterval;

        [Header("Amounts")]
        [SerializeField] private RewardDefinitionSO _cashReward;
        [SerializeField] private int _cashBase;
        [SerializeField] private int _cashPerZone;
        [SerializeField] private int _roundingThreshold;
        [SerializeField] private int _roundingStep;

        [Header("Wheels")]
        [SerializeField] private int _sliceCount;
        [SerializeField] private ZoneBand[] _bands;
        [SerializeField] private WheelContentSO _superContent;
        [SerializeField] private ZoneOverride[] _overrides;

        public int MaxZone => _maxZone;
        public int SafeZoneInterval => _safeZoneInterval;
        public int SuperZoneInterval => _superZoneInterval;
        public RewardDefinitionSO CashReward => _cashReward;
        public int CashBase => _cashBase;
        public int CashPerZone => _cashPerZone;
        public int RoundingThreshold => _roundingThreshold;
        public int RoundingStep => _roundingStep;
        public int SliceCount => _sliceCount;

        public ZoneType GetZoneType(int zone) {
            if (zone % _superZoneInterval == 0) return ZoneType.Super;
            if (zone % _safeZoneInterval == 0) return ZoneType.Safe;
            return ZoneType.Normal;
        }

        public WheelContentSO GetContent(int zone) {
            foreach (var zoneOverride in _overrides) {
                if (zoneOverride.Zone == zone) return zoneOverride.Content;
            }

            if (GetZoneType(zone) == ZoneType.Super) return _superContent;

            foreach (var band in _bands) {
                if (zone <= band.UntilZone) return band.Content;
            }

            return _bands[_bands.Length - 1].Content;
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            if (_maxZone <= 0 || _safeZoneInterval <= 0 || _superZoneInterval <= 0 || _sliceCount <= 0 || _roundingStep <= 0) {
                Debug.LogWarning($"{name}: zone counts, intervals, slice count and rounding step must be above 0.", this);
                return;
            }

            if (_roundingStep > _cashPerZone) Debug.LogWarning($"{name}: rounding step is larger than the cash growth per zone, so neighbouring zones can pay the same cash.", this);
            if (_cashReward == null) Debug.LogWarning($"{name}: no cash reward is set.", this);

            ValidateBands();
            ValidateContent(_superContent, "super content", requiresBomb: false, allowsBomb: false);
            ValidateOverrides();
        }

        private void ValidateBands() {
            if (_bands == null || _bands.Length == 0) {
                Debug.LogWarning($"{name}: no bands are set.", this);
                return;
            }

            int previousUntil = 0;
            for (int i = 0; i < _bands.Length; i++) {
                if (_bands[i].UntilZone <= previousUntil) Debug.LogWarning($"{name}: band {i + 1} must end after zone {previousUntil}.", this);
                previousUntil = _bands[i].UntilZone;
                ValidateContent(_bands[i].Content, $"band {i + 1}", requiresBomb: true, allowsBomb: true);
            }

            if (previousUntil < _maxZone) Debug.LogWarning($"{name}: the last band ends at zone {previousUntil}, before the last zone {_maxZone}.", this);
        }

        private void ValidateOverrides() {
            if (_overrides == null) return;

            var seenZones = new HashSet<int>();
            foreach (var zoneOverride in _overrides) {
                int zone = zoneOverride.Zone;
                if (zone < FirstZone || zone > _maxZone) Debug.LogWarning($"{name}: override for zone {zone} is outside zones {FirstZone}–{_maxZone}.", this);
                if (!seenZones.Add(zone)) Debug.LogWarning($"{name}: zone {zone} is overridden more than once.", this);

                var zoneType = GetZoneType(zone);
                if (zoneType == ZoneType.Super) Debug.LogWarning($"{name}: override for zone {zone} replaces the super zone's content.", this);
                ValidateContent(zoneOverride.Content, $"override for zone {zone}", requiresBomb: zoneType == ZoneType.Normal, allowsBomb: zoneType != ZoneType.Super);
            }
        }

        private void ValidateContent(WheelContentSO content, string label, bool requiresBomb, bool allowsBomb) {
            if (content == null) {
                Debug.LogWarning($"{name}: {label} has no wheel content.", this);
                return;
            }

            if (content.Slices.Count != _sliceCount) Debug.LogWarning($"{name}: {label} ({content.name}) has {content.Slices.Count} slices; the wheel has {_sliceCount}.", this);

            int bombCount = 0;
            foreach (var slice in content.Slices) {
                if (slice.Type == SliceType.Bomb) bombCount++;
            }

            if (requiresBomb && bombCount != 1) Debug.LogWarning($"{name}: {label} ({content.name}) needs exactly one bomb slice.", this);
            if (!allowsBomb && bombCount > 0) Debug.LogWarning($"{name}: {label} ({content.name}) must not have a bomb slice.", this);
        }
    #endif
    }
}
