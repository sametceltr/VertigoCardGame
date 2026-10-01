using TMPro;
using UnityEngine;

namespace CardGame.Zones.View
{
    public class ZoneItem : MonoBehaviour
    {
        private const string NameFormat = "ui_text_zone_{0:00}";

        [SerializeField] private TextMeshProUGUI _zoneText;
        [SerializeField] private RectTransform _rectTransform;

        private ZoneVisuals _visuals;

        public RectTransform RectTransform => _rectTransform;

        public void Initialize(int zone, ZoneVisuals visuals) {
            _visuals = visuals;
            name = string.Format(NameFormat, zone);
            _zoneText.text = zone.ToString();
            SetCurrent(false);
        }

        public void SetCurrent(bool isCurrent) {
            _zoneText.color = isCurrent ? _visuals.CurrentZoneNumberColor : _visuals.ZoneNumberColor;
        }
    }
}
