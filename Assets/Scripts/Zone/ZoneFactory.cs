using System.Collections.Generic;
using System.Linq;

public class ZoneFactory
{
    private readonly IRewardGenerationStrategy rewardStrategy;
    private readonly Dictionary<int, Zone> predefinedZones;

    public ZoneFactory(IRewardGenerationStrategy strategy, ZoneSO[] predefinedZoneSOs) {
        rewardStrategy = strategy;
        predefinedZones = predefinedZoneSOs?.ToDictionary(
            item => item.Zone.Index,
            item => item.Zone
        ) ?? new Dictionary<int, Zone>();
    }

    public Zone CreateZone(int zoneIndex) {
        if (predefinedZones.TryGetValue(zoneIndex, out var predefinedZone)) {
            predefinedZone.Initialize(predefinedZone.Index);
            return predefinedZone;
        }

        var zone = new Zone(zoneIndex);
        var rewards = rewardStrategy.GenerateRewards(zone);
        zone.SetRewards(rewards);

        return zone;
    }
}