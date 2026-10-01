using System.Collections.Generic;
using CardGame.Core.Randomness;
using CardGame.Rewards;
using CardGame.Zones;

namespace CardGame.Wheel
{
    public class WheelBuilder
    {
        private readonly ZoneProgressionSO _progression;
        private readonly AmountCalculator _amountCalculator;
        private readonly IRandom _random;

        public WheelBuilder(ZoneProgressionSO progression, AmountCalculator amountCalculator, IRandom random) {
            _progression = progression;
            _amountCalculator = amountCalculator;
            _random = random;
        }

        public IReadOnlyList<WheelSlice> Build(int zone) {
            var content = _progression.GetContent(zone);
            bool isNormalZone = _progression.GetZoneType(zone) == ZoneType.Normal;
            var slices = new WheelSlice[content.Slices.Count];
            var usedRewards = new HashSet<RewardDefinitionSO>();
            var rewardSliceIndices = new List<int>();

            for (int i = 0; i < content.Slices.Count; i++) {
                var entry = content.Slices[i];
                if (entry.Type == SliceType.Bomb) continue;

                var reward = DrawReward(entry.Pool, usedRewards);
                slices[i] = WheelSlice.ForReward(reward, _amountCalculator.AmountFor(reward, zone), entry.LandingWeight);
                rewardSliceIndices.Add(i);
            }

            for (int i = 0; i < content.Slices.Count; i++) {
                var entry = content.Slices[i];
                if (entry.Type != SliceType.Bomb) continue;

                if (isNormalZone) {
                    slices[i] = WheelSlice.ForBomb(entry.LandingWeight);
                } else {
                    var reward = DrawFromWheelPools(content, rewardSliceIndices, usedRewards);
                    slices[i] = WheelSlice.ForReward(reward, _amountCalculator.AmountFor(reward, zone), entry.LandingWeight);
                }
            }

            return slices;
        }

        private RewardDefinitionSO DrawFromWheelPools(WheelContentSO content, List<int> rewardSliceIndices, HashSet<RewardDefinitionSO> usedRewards) {
            var candidates = new List<int>(rewardSliceIndices);
            while (candidates.Count > 0) {
                int pick = _random.Range(0, candidates.Count);
                var pool = content.Slices[candidates[pick]].Pool;
                if (HasUnusedReward(pool, usedRewards)) return DrawReward(pool, usedRewards);
                candidates.RemoveAt(pick);
            }

            return DrawReward(content.Slices[rewardSliceIndices[0]].Pool, usedRewards);
        }

        private RewardDefinitionSO DrawReward(IReadOnlyList<PoolEntry> pool, HashSet<RewardDefinitionSO> usedRewards) {
            var weights = new int[pool.Count];
            bool hasUnused = HasUnusedReward(pool, usedRewards);
            for (int i = 0; i < pool.Count; i++) {
                bool blocked = hasUnused && usedRewards.Contains(pool[i].Reward);
                weights[i] = blocked ? 0 : pool[i].AppearanceWeight;
            }

            var reward = pool[WeightedPicker.Pick(weights, _random)].Reward;
            usedRewards.Add(reward);
            return reward;
        }

        private static bool HasUnusedReward(IReadOnlyList<PoolEntry> pool, HashSet<RewardDefinitionSO> usedRewards) {
            foreach (var entry in pool) {
                if (entry.AppearanceWeight > 0 && !usedRewards.Contains(entry.Reward)) return true;
            }

            return false;
        }
    }
}
