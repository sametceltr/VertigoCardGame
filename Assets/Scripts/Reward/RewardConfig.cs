using System;
using UnityEngine;

[Serializable]
public class RewardConfig
{
    [SerializeField] RewardType _rewardType;
    [SerializeField] Sprite _iconSprite;
    [SerializeField] int _baseMultiplier = 1;

    public RewardType RewardType => _rewardType;
    public Sprite IconSprite => _iconSprite;

    public int BaseMultiplier => _baseMultiplier; 
}