using System.Collections.Generic;
using CardGame.Wheel;
using CardGame.Zones;

namespace CardGame.Run
{
    public readonly struct ZoneInfo
    {
        public int Zone { get; }
        public ZoneType Type { get; }
        public IReadOnlyList<WheelSlice> Slices { get; }

        public ZoneInfo(int zone, ZoneType type, IReadOnlyList<WheelSlice> slices) {
            Zone = zone;
            Type = type;
            Slices = slices;
        }
    }
}
