using System;
using UnityEngine;

[Serializable]
public class WheelConfig
{
    [SerializeField] WheelType _wheelType;
    [SerializeField] Sprite _baseSprite;
    [SerializeField] Sprite _indicatorSprite;
    [SerializeField] Color _titleColor = Color.white;

    public WheelType WheelType => _wheelType;
    public Sprite BaseSprite => _baseSprite;
    public Sprite IndicatorSprite => _indicatorSprite;
    public Color TitleColor => _titleColor;

}