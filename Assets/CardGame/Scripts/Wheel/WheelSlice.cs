using CardGame.Rewards;

namespace CardGame.Wheel
{
    public readonly struct WheelSlice
    {
        public SliceType Type { get; }
        public RewardDefinitionSO Reward { get; }
        public int Amount { get; }
        public int LandingWeight { get; }

        private WheelSlice(SliceType type, RewardDefinitionSO reward, int amount, int landingWeight) {
            Type = type;
            Reward = reward;
            Amount = amount;
            LandingWeight = landingWeight;
        }

        public static WheelSlice ForReward(RewardDefinitionSO reward, int amount, int landingWeight) {
            return new WheelSlice(SliceType.Reward, reward, amount, landingWeight);
        }

        public static WheelSlice ForBomb(int landingWeight) {
            return new WheelSlice(SliceType.Bomb, null, 0, landingWeight);
        }
    }
}
