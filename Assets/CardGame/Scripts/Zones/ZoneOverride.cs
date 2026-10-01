using System;
using CardGame.Wheel;
using UnityEngine;

namespace CardGame.Zones
{
    [Serializable]
    public class ZoneOverride
    {
        [SerializeField] private int _zone;
        [SerializeField] private WheelContentSO _content;

        public int Zone => _zone;
        public WheelContentSO Content => _content;
    }
}
