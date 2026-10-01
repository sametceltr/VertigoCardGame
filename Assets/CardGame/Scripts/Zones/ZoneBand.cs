using System;
using CardGame.Wheel;
using UnityEngine;

namespace CardGame.Zones
{
    [Serializable]
    public class ZoneBand
    {
        [SerializeField] private int _untilZone;
        [SerializeField] private WheelContentSO _content;

        public int UntilZone => _untilZone;
        public WheelContentSO Content => _content;
    }
}
