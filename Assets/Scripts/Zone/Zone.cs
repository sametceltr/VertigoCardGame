using System;
using UnityEngine;

[Serializable]
public class Zone
{
    [SerializeField] int _index;
    [SerializeField] Reward[] _sliceRewards = new Reward[8];
    private ZoneType _zoneType;
    private WheelType _wheelType;

    public Zone(int zoneIndex) {
        Initialize(zoneIndex);
    }

    public void Initialize(int zoneIndex) {
        _index = zoneIndex;
        DecideTypes(zoneIndex);
    }

    public void SetRewards(Reward[] sliceRewards) {
        _sliceRewards = sliceRewards;
    }

    private void DecideTypes(int zoneIndex) {
        if (zoneIndex % 30 == 0) {
            _zoneType = ZoneType.SUPER;
            _wheelType = WheelType.GOLDEN;
            return;
        }
        else if (zoneIndex % 5 == 0) {
            _zoneType = ZoneType.SAFE;
            _wheelType = WheelType.SILVER;
            return;
        }
        _zoneType = ZoneType.NORMAL;
        _wheelType = WheelType.BRONZE;
    }

    public int Index => _index;
    public Reward[] SliceEntries => _sliceRewards;

    public ZoneType Type => _zoneType;

    public WheelType WheelType => _wheelType;
}
