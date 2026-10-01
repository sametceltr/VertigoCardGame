using CardGame.Rewards;
using CardGame.Zones;

namespace CardGame.Wheel
{
    public class AmountCalculator
    {
        private readonly ZoneProgressionSO _progression;

        public AmountCalculator(ZoneProgressionSO progression) {
            _progression = progression;
        }

        public int AmountFor(RewardDefinitionSO reward, int zone) {
            int multiplier = _progression.GetZoneType(zone) == ZoneType.Normal ? 1 : _progression.SafeAndSuperMultiplier;

            int amount = reward == _progression.CashReward
                ? (_progression.CashBase + _progression.CashPerZone * zone) * multiplier
                : TierOf(zone) * multiplier;

            return RoundDown(amount);
        }

        private int TierOf(int zone) {
            return (zone + _progression.TierSize - 1) / _progression.TierSize;
        }

        private int RoundDown(int amount) {
            if (amount < _progression.RoundingThreshold) return amount;
            return amount - amount % _progression.RoundingStep;
        }
    }
}
