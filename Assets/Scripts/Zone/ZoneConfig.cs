using System;
using UnityEngine;

[Serializable]
public class ZoneConfig
{
    [SerializeField] ZoneType _zoneType;
    [SerializeField] Color _normalTextColor = Color.white;
    [SerializeField] Color _indicatorTextColor = Color.white;
    [SerializeField] Color _indicatorBackgroundSpriteColor = Color.white;
    [SerializeField] Sprite _indicatorBackgroundSprite;

    public ZoneType ZoneType => _zoneType;

    public Color NormalTextColor => _normalTextColor;
    public Color IndicatorTextColor => _indicatorTextColor;
    public Color IndicatorBackgroundSpriteColor => _indicatorBackgroundSpriteColor;
    public Sprite IndicatorBackgroundSprite => _indicatorBackgroundSprite;


}
