using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZoneItem : MonoBehaviour
{
    [SerializeField] private ZoneConfigSO zoneConfigs;
    [SerializeField] private TextMeshProUGUI zoneText;
    [SerializeField] private RectTransform rectTransform;
    private Image _indicatorBackground;

    private Zone _zone;

    public RectTransform RectTransform => rectTransform;
    public Zone Zone => _zone;

    public void Initialize(Zone zone, Image indicatorBackground) {
        _zone = zone;
        _indicatorBackground = indicatorBackground;
        gameObject.name = "Level_" + _zone.Index.ToString();
        zoneText.text = _zone.Index.ToString();
        Configure(zone.Index);
    }

    public void Configure(int zoneIndex) {
        if (_zone == null) return;

        var zoneConfig = zoneConfigs.GetConfig(_zone.Type);

        if (_zone.Index == zoneIndex) {
            zoneText.color = zoneConfig.IndicatorTextColor;
            _indicatorBackground.sprite = zoneConfig.IndicatorBackgroundSprite;
            _indicatorBackground.color = zoneConfig.IndicatorBackgroundSpriteColor;
            return;
        }

        zoneText.color = zoneConfig.NormalTextColor;
    }

}
