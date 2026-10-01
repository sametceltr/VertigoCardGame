using System;
using UnityEngine;

namespace CardGame.Zones
{
    [Serializable]
    public class ZoneVisuals
    {
        [Header("Wheel")]
        [SerializeField] private string _title;
        [SerializeField] private Color _titleColor;
        [SerializeField] private Sprite _wheelBase;
        [SerializeField] private Sprite _wheelIndicator;

        [Header("Zone Bar")]
        [SerializeField] private Color _zoneNumberColor;
        [SerializeField] private Color _currentZoneNumberColor;
        [SerializeField] private Sprite _currentZoneBoxSprite;
        [SerializeField] private Color _currentZoneBoxColor;

        public string Title => _title;
        public Color TitleColor => _titleColor;
        public Sprite WheelBase => _wheelBase;
        public Sprite WheelIndicator => _wheelIndicator;
        public Color ZoneNumberColor => _zoneNumberColor;
        public Color CurrentZoneNumberColor => _currentZoneNumberColor;
        public Sprite CurrentZoneBoxSprite => _currentZoneBoxSprite;
        public Color CurrentZoneBoxColor => _currentZoneBoxColor;
    }
}
