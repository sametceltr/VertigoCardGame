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
            return RoundDown(reward.BaseAmount + reward.AmountPerZone * zone);
        }

        private int RoundDown(int amount) {
            if (amount < _progression.RoundingThreshold) return amount;
            return amount - amount % _progression.RoundingStep;
        }
    }
}
